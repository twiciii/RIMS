using Microsoft.EntityFrameworkCore;
using RIMS.Data;
using RIMS.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace RIMS.Services
{
    public interface IHouseholdService
    {
        Task<HouseholdDto?> CreateOrUpdateHouseholdAsync(HouseholdDto householdDto);
        Task<HouseholdDto?> GetHouseholdByHeadAsync(int headOfHouseholdId);
        Task<List<HouseholdMemberDto>> GetHouseholdMembersAsync(int headOfHouseholdId);
        Task<bool> RemoveHouseholdMemberAsync(int residentId);
    }

    public class HouseholdService : IHouseholdService
    {
        private readonly ApplicationDbContext _context;

        public HouseholdService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HouseholdDto?> CreateOrUpdateHouseholdAsync(HouseholdDto householdDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Update Head of Household information
                var headResident = await _context.rimsResidents
                    .FirstOrDefaultAsync(r => r.Id == householdDto.HeadOfHouseholdId);

                if (headResident != null)
                {
                    headResident.IsHeadOfHousehold = true;
                    headResident.NumberOfDependents = householdDto.NumberOfDependents;
                }

                // 2. Update household members' relationships
                foreach (var member in householdDto.HouseholdMembers)
                {
                    var resident = await _context.rimsResidents
                        .FirstOrDefaultAsync(r => r.Id == member.ResidentId);

                    if (resident != null)
                    {
                        resident.HeadOfHouseholdId = householdDto.HeadOfHouseholdId;
                    }

                    // Update relationship in DocumentApplication
                    var documentApp = await _context.rimsDocumentApplication
                        .FirstOrDefaultAsync(da => da.FK_ResidentId == member.ResidentId);

                    if (documentApp != null)
                    {
                        documentApp.Relationship = member.Relationship;
                    }
                    else
                    {
                        // Create new DocumentApplication if doesn't exist
                        var newApp = new RIMSDocumentApplication
                        {
                            FK_ResidentId = member.ResidentId,
                            Relationship = member.Relationship,
                            // Set other required fields with default values
                            EmailAddress = "default@example.com",
                            CivilStatus = "Single",
                            ResidencyStatus = "Owner",
                            Religion = "Catholic",
                            Occupation = "Not Specified",
                            EducationalAttainment = "High School",
                            EmploymentStatus = "Unemployed",
                            MonthlyIncome = "₱0 - ₱5,000",
                            ContactNumber = "09000000000",
                            FK_DocumentId = 1, // Default document ID
                            ApplicationNumber = GenerateApplicationNumber(),
                            Status = "Pending",
                            Purpose = "Household registration",
                            CreatedBy = "System"
                        };
                        await _context.rimsDocumentApplication.AddAsync(newApp);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetHouseholdByHeadAsync(householdDto.HeadOfHouseholdId);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<HouseholdDto?> GetHouseholdByHeadAsync(int headOfHouseholdId)
        {
            var headResident = await _context.rimsResidents
                .Include(r => r.Dependents)
                .FirstOrDefaultAsync(r => r.Id == headOfHouseholdId && r.IsHeadOfHousehold);

            if (headResident == null)
                return null;

            var householdMembers = await GetHouseholdMembersAsync(headOfHouseholdId);

            return new HouseholdDto
            {
                HeadOfHouseholdId = headResident.Id,
                HeadFirstName = headResident.FirstName,
                HeadMiddleName = headResident.MiddleName ?? string.Empty,
                HeadLastName = headResident.LastName,
                NumberOfDependents = headResident.NumberOfDependents ?? 0,
                HouseholdMembers = householdMembers
            };
        }

        public async Task<List<HouseholdMemberDto>> GetHouseholdMembersAsync(int headOfHouseholdId)
        {
            var members = await _context.rimsResidents
                .Where(r => r.HeadOfHouseholdId == headOfHouseholdId || r.Id == headOfHouseholdId)
                .Select(r => new
                {
                    Resident = r,
                    DocumentApp = _context.rimsDocumentApplication
                        .FirstOrDefault(da => da.FK_ResidentId == r.Id)
                })
                .ToListAsync();

            return members.Select(m => new HouseholdMemberDto
            {
                ResidentId = m.Resident.Id,
                FirstName = m.Resident.FirstName,
                MiddleName = m.Resident.MiddleName ?? string.Empty,
                LastName = m.Resident.LastName,
                Relationship = m.Resident.Id == headOfHouseholdId ?
                    "Head" : m.DocumentApp?.Relationship ?? "Family Member",
                IsHeadOfHousehold = m.Resident.Id == headOfHouseholdId,
                DateOfBirth = m.Resident.DateOfBirth,
                Age = CalculateAge(m.Resident.DateOfBirth)
            }).ToList();
        }

        public async Task<bool> RemoveHouseholdMemberAsync(int residentId)
        {
            var resident = await _context.rimsResidents
                .FirstOrDefaultAsync(r => r.Id == residentId);

            if (resident != null)
            {
                resident.HeadOfHouseholdId = null;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        private static string GenerateApplicationNumber()
        {
            return $"APP-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }

    public class HouseholdDto
    {
        public int HeadOfHouseholdId { get; set; }
        public required string HeadFirstName { get; set; }
        public required string HeadMiddleName { get; set; }
        public required string HeadLastName { get; set; }
        public int NumberOfDependents { get; set; }
        public List<HouseholdMemberDto> HouseholdMembers { get; set; } = new();
    }

    public class HouseholdMemberDto
    {
        public int ResidentId { get; set; }
        public required string FirstName { get; set; }
        public required string MiddleName { get; set; }
        public required string LastName { get; set; }
        public required string Relationship { get; set; }
        public bool IsHeadOfHousehold { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
    }
}