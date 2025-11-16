using Microsoft.EntityFrameworkCore;
using RIMS.Data;
using RIMS.Models.Entities;
using RIMS.Models.ViewModels;
using RIMS.Repositories;
using RIMS.Services.Interfaces;

namespace RIMS.Services.Implementations
{
    public class ResidentService : IResidentService
    {
        private readonly IResidentRepository _residentRepository;
        private readonly ApplicationDbContext _context;

        public ResidentService(
            IResidentRepository residentRepository,
            ApplicationDbContext context)
        {
            _residentRepository = residentRepository;
            _context = context;
        }

        // Basic CRUD operations
        public async Task<RIMSResident?> GetResidentByIdAsync(int id)
        {
            try
            {
                return await _residentRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting resident by ID: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<RIMSResident>> GetAllResidentsAsync()
        {
            try
            {
                return await _residentRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting all residents: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<RIMSResident>> GetResidentsByCategoryAsync(int categoryId)
        {
            try
            {
                return await _residentRepository.GetByCategoryAsync(categoryId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting residents by category: {ex.Message}", ex);
            }
        }

        public async Task<RIMSResident> CreateResidentAsync(RIMSResident resident)
        {
            try
            {
                var result = await _residentRepository.AddAsync(resident);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating resident: {ex.Message}", ex);
            }
        }

        public async Task<RIMSResident> UpdateResidentAsync(RIMSResident resident)
        {
            try
            {
                var result = await _residentRepository.UpdateAsync(resident);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating resident: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteResidentAsync(int id)
        {
            try
            {
                var result = await _residentRepository.DeleteAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting resident: {ex.Message}", ex);
            }
        }

        // Search and analytics
        public async Task<IEnumerable<RIMSResident>> SearchResidentsAsync(string searchTerm)
        {
            try
            {
                return await _residentRepository.SearchAsync(searchTerm ?? string.Empty);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching residents: {ex.Message}", ex);
            }
        }

        public async Task<int> GetResidentCountAsync()
        {
            try
            {
                return await _residentRepository.GetCountAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting resident count: {ex.Message}", ex);
            }
        }

        // Detailed operations
        public async Task<RIMSResident?> GetResidentWithDetailsAsync(int id)
        {
            try
            {
                return await _residentRepository.GetWithDetailsAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting resident with details: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<RIMSResidentCategory>> GetResidentCategoriesAsync(int residentId)
        {
            try
            {
                return await _context.rimsResidentCategories
                    .Where(rc => rc.ResidentId == residentId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting resident categories: {ex.Message}", ex);
            }
        }

        public async Task<bool> AddResidentCategoryAsync(RIMSResidentCategory category)
        {
            try
            {
                _context.rimsResidentCategories.Add(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding resident category: {ex.Message}", ex);
            }
        }

        public async Task<bool> RemoveResidentCategoryAsync(int categoryId)
        {
            try
            {
                var category = await _context.rimsResidentCategories.FindAsync(categoryId);
                if (category != null)
                {
                    _context.rimsResidentCategories.Remove(category);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error removing resident category: {ex.Message}", ex);
            }
        }

        // Advanced operations with DTOs
        public async Task<RIMSResident> CreateResidentWithDetailsAsync(ResidentCreateDto residentDto)
        {
            if (residentDto == null)
                throw new ArgumentNullException(nameof(residentDto));

            try
            {
                var resident = new RIMSResident
                {
                    FirstName = residentDto.FirstName ?? string.Empty,
                    MiddleName = residentDto.MiddleName ?? string.Empty,
                    LastName = residentDto.LastName ?? string.Empty,
                    Suffix = residentDto.Suffix ?? string.Empty,
                    DateOfBirth = residentDto.DateOfBirth,
                    PlaceOfBirth = residentDto.PlaceOfBirth ?? string.Empty,
                    Sex = residentDto.Sex ?? string.Empty,
                    Gender = residentDto.Sex ?? string.Empty, // Using Sex for Gender as per DbContext
                    Nationality = residentDto.Nationality ?? string.Empty,
                    Address = residentDto.Address ?? string.Empty,
                    ContactNumber = residentDto.ContactNumber ?? string.Empty,
                    Email = residentDto.EmailAddress ?? string.Empty,
                    CivilStatus = residentDto.CivilStatus ?? string.Empty,
                    Occupation = residentDto.Occupation ?? string.Empty
                };

                _context.rimsResidents.Add(resident);
                await _context.SaveChangesAsync();

                // Create DocumentApplication if purpose is provided
                if (!string.IsNullOrEmpty(residentDto.Purpose))
                {
                    var documentApp = new RIMSDocumentApplication
                    {
                        FK_ResidentId = resident.Id, // Using Id instead of ResidentId
                        EmailAddress = residentDto.EmailAddress ?? string.Empty,
                        CivilStatus = residentDto.CivilStatus ?? string.Empty,
                        ResidencyStatus = residentDto.ResidencyStatus ?? string.Empty,
                        Religion = residentDto.Religion ?? string.Empty,
                        Relationship = residentDto.Relationship ?? string.Empty,
                        Occupation = residentDto.Occupation ?? string.Empty,
                        EducationalAttainment = residentDto.EducationalAttainment ?? string.Empty,
                        EmploymentStatus = residentDto.EmploymentStatus ?? string.Empty,
                        MonthlyIncome = residentDto.MonthlyIncome ?? string.Empty,
                        ContactNumber = residentDto.ContactNumber ?? string.Empty,
                        Purpose = residentDto.Purpose ?? string.Empty,
                        FK_DocumentId = 1, // Default document ID
                        DateInsurance = DateTime.Now, // Using DateInsurance as timestamp
                        Status = "Pending"
                    };

                    _context.rimsDocumentApplication.Add(documentApp);
                    await _context.SaveChangesAsync();
                }

                return resident;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating resident with details: {ex.Message}", ex);
            }
        }

        public async Task<ResidentFullDto> GetResidentFullDetailsAsync(int residentId)
        {
            try
            {
                var resident = await _context.rimsResidents
                    .Include(r => r.DocumentApplications)
                    .Include(r => r.ResidentCategories)
                    .FirstOrDefaultAsync(r => r.Id == residentId);

                if (resident == null)
                    return new ResidentFullDto();

                var documentApp = resident.DocumentApplications?.FirstOrDefault();
                var categories = resident.ResidentCategories?.ToList() ?? new List<RIMSResidentCategory>();

                var result = new ResidentFullDto
                {
                    ResidentId = resident.Id,
                    FirstName = resident.FirstName ?? string.Empty,
                    MiddleName = resident.MiddleName ?? string.Empty,
                    LastName = resident.LastName ?? string.Empty,
                    Suffix = resident.Suffix ?? string.Empty,
                    DateOfBirth = resident.DateOfBirth,
                    PlaceOfBirth = resident.PlaceOfBirth ?? string.Empty,
                    Sex = resident.Sex ?? string.Empty,
                    Age = resident.Age, // Direct assignment since Age is not nullable
                    Nationality = resident.Nationality ?? string.Empty,
                    Address = resident.Address ?? string.Empty,
                    ContactNumber = resident.ContactNumber ?? string.Empty,
                    EmailAddress = resident.Email ?? string.Empty,
                    CivilStatus = resident.CivilStatus ?? string.Empty,

                    // From DocumentApplication - using safe null checks
                    ResidencyStatus = documentApp?.ResidencyStatus ?? string.Empty,
                    Religion = documentApp?.Religion ?? string.Empty,
                    Relationship = documentApp?.Relationship ?? string.Empty,
                    EducationalAttainment = documentApp?.EducationalAttainment ?? string.Empty,
                    EmploymentStatus = documentApp?.EmploymentStatus ?? string.Empty,
                    MonthlyIncome = documentApp?.MonthlyIncome ?? string.Empty,
                    Purpose = documentApp?.Purpose ?? string.Empty,

                    // Categories - using safe null checks
                    IsPWD = categories.Any(c => c.CategoryName != null && c.CategoryName.Contains("PWD")),
                    Is4PsMember = categories.Any(c => c.CategoryName != null && c.CategoryName.Contains("4Ps")),
                    IsSeniorCitizen = categories.Any(c => c.CategoryName != null && c.CategoryName.Contains("Senior")),
                    IsSoloParent = categories.Any(c => c.CategoryName != null && c.CategoryName.Contains("Solo Parent"))
                };

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting resident full details: {ex.Message}", ex);
            }
        }

        // DTO methods for backward compatibility
        public async Task<RIMSResident> CreateResidentWithHouseholdAsync(ResidentCreateDto residentDto)
        {
            return await CreateResidentWithDetailsAsync(residentDto);
        }

        public async Task<ResidentFullDto> GetResidentWithHouseholdAsync(int residentId)
        {
            return await GetResidentFullDetailsAsync(residentId);
        }
    }
}