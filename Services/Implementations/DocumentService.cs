using Microsoft.EntityFrameworkCore;
using RIMS.Data;
using RIMS.Models.Entities;
using RIMS.Services.Interfaces;

namespace RIMS.Services.Implementations
{
    public class DocumentService : IDocumentService
    {
        private readonly ApplicationDbContext _context;

        public DocumentService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Document operations
        public async Task<RIMSDocument> GetDocumentByIdAsync(int id)
        {
            var document = await _context.rimsDocuments.FindAsync(id);
            return document ?? throw new KeyNotFoundException($"Document with ID {id} not found");
        }

        public async Task<IEnumerable<RIMSDocument>> GetAllDocumentsAsync()
        {
            return await _context.rimsDocuments.ToListAsync();
        }

        public async Task<RIMSDocument> CreateDocumentAsync(RIMSDocument document)
        {
            _context.rimsDocuments.Add(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task<RIMSDocument> UpdateDocumentAsync(RIMSDocument document)
        {
            _context.rimsDocuments.Update(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task<bool> DeleteDocumentAsync(int id)
        {
            var document = await _context.rimsDocuments.FindAsync(id);
            if (document != null)
            {
                _context.rimsDocuments.Remove(document);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Document Application operations
        public async Task<RIMSDocumentApplication> GetDocumentApplicationByIdAsync(int id)
        {
            var application = await _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Include(da => da.Document)
                .Include(da => da.Address)
                .FirstOrDefaultAsync(da => da.Id == id);

            return application ?? throw new KeyNotFoundException($"Document application with ID {id} not found");
        }

        public async Task<IEnumerable<RIMSDocumentApplication>> GetAllDocumentApplicationsAsync()
        {
            return await _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Include(da => da.Document)
                .ToListAsync();
        }

        public async Task<IEnumerable<RIMSDocumentApplication>> GetPendingApplicationsAsync()
        {
            return await _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Include(da => da.Document)
                .Where(da => da.Status == "Pending")
                .ToListAsync();
        }

        public async Task<IEnumerable<RIMSDocumentApplication>> GetCompletedApplicationsAsync()
        {
            return await _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Include(da => da.Document)
                .Where(da => da.Status == "Completed")
                .ToListAsync();
        }

        public async Task<IEnumerable<RIMSDocumentApplication>> GetCertificateApplicationsAsync()
        {
            return await _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Include(da => da.Document)
                .Where(da => da.Document != null && da.Document.DocumentName != null && da.Document.DocumentName.Contains("Certificate"))
                .ToListAsync();
        }

        public async Task<IEnumerable<RIMSDocumentApplication>> GetPendingCertificateApplicationsAsync()
        {
            return await _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Include(da => da.Document)
                .Where(da => da.Status == "Pending" && da.Document != null && da.Document.DocumentName != null && da.Document.DocumentName.Contains("Certificate"))
                .ToListAsync();
        }

        public async Task<IEnumerable<RIMSDocumentApplication>> GetTodayCompletedApplicationsAsync()
        {
            var today = DateTime.Today;
            return await _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Include(da => da.Document)
                .Where(da => da.Status == "Completed" &&
                            da.DateInsurance.HasValue &&
                            da.DateInsurance.Value.Date == today)
                .ToListAsync();
        }

        public async Task<IEnumerable<RIMSDocumentApplication>> GetCompletedApplicationsByDateAsync(DateTime date)
        {
            return await _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Include(da => da.Document)
                .Where(da => da.Status == "Completed" &&
                            da.DateInsurance.HasValue &&
                            da.DateInsurance.Value.Date == date.Date)
                .ToListAsync();
        }

        public async Task<RIMSDocumentApplication> CreateDocumentApplicationAsync(RIMSDocumentApplication application)
        {
            application.Status = "Pending";
            application.DateInsurance = DateTime.Now;
            application.ApplicationDate = DateTime.Now;

            _context.rimsDocumentApplication.Add(application);
            await _context.SaveChangesAsync();
            return application;
        }

        public async Task<RIMSDocumentApplication> UpdateDocumentApplicationAsync(RIMSDocumentApplication application)
        {
            _context.rimsDocumentApplication.Update(application);
            await _context.SaveChangesAsync();
            return application;
        }

        public async Task<bool> DeleteDocumentApplicationAsync(int id)
        {
            var application = await _context.rimsDocumentApplication.FindAsync(id);
            if (application != null)
            {
                _context.rimsDocumentApplication.Remove(application);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ArchiveApplicationAsync(int id)
        {
            var application = await _context.rimsDocumentApplication.FindAsync(id);
            if (application != null)
            {
                application.Status = "Archived";
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Application processing
        public async Task<bool> ApproveApplicationAsync(int applicationId)
        {
            var application = await _context.rimsDocumentApplication.FindAsync(applicationId);
            if (application != null)
            {
                application.Status = "Approved";
                application.ApprovedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RejectApplicationAsync(int applicationId, string remarks)
        {
            var application = await _context.rimsDocumentApplication.FindAsync(applicationId);
            if (application != null)
            {
                application.Status = "Rejected";
                application.RejectedDate = DateTime.Now;
                application.RejectionRemarks = remarks ?? string.Empty;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RejectCertificateAsync(int applicationId, string remarks)
        {
            var application = await _context.rimsDocumentApplication
                .Include(da => da.Document)
                .FirstOrDefaultAsync(da => da.Id == applicationId);

            if (application != null && application.Document?.DocumentName?.Contains("Certificate") == true)
            {
                application.Status = "Rejected";
                application.RejectedDate = DateTime.Now;
                application.RejectionRemarks = remarks ?? string.Empty;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ReturnApplicationAsync(int applicationId, string feedback)
        {
            var application = await _context.rimsDocumentApplication.FindAsync(applicationId);
            if (application != null)
            {
                application.Status = "Returned";
                application.Feedback = feedback ?? string.Empty;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> IssueCertificateAsync(int applicationId)
        {
            var application = await _context.rimsDocumentApplication
                .Include(da => da.Document)
                .FirstOrDefaultAsync(da => da.Id == applicationId);

            if (application != null && application.Document?.DocumentName?.Contains("Certificate") == true)
            {
                application.Status = "Issued";
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Search and filtering
        public async Task<IEnumerable<RIMSDocumentApplication>> SearchDocumentApplicationsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllDocumentApplicationsAsync();

            return await _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Include(da => da.Document)
                .Where(da =>
                    (da.Resident != null && da.Resident.FirstName != null && da.Resident.FirstName.Contains(searchTerm)) ||
                    (da.Resident != null && da.Resident.LastName != null && da.Resident.LastName.Contains(searchTerm)) ||
                    (da.Document != null && da.Document.DocumentName != null && da.Document.DocumentName.Contains(searchTerm)) ||
                    (da.Status != null && da.Status.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<IEnumerable<RIMSDocumentApplication>> SearchByDocumentTypeAsync(string documentType)
        {
            return await _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Include(da => da.Document)
                .Where(da => da.Document != null && da.Document.DocumentName != null && da.Document.DocumentName.Contains(documentType))
                .ToListAsync();
        }

        public async Task<IEnumerable<RIMSDocumentApplication>> SearchByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            return await _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Include(da => da.Document)
                .Where(da => da.DateInsurance.HasValue && da.DateInsurance.Value >= fromDate && da.DateInsurance.Value <= toDate)
                .ToListAsync();
        }

        // Statistics
        public async Task<int> GetPendingApplicationsCountAsync()
        {
            return await _context.rimsDocumentApplication
                .Where(da => da.Status == "Pending")
                .CountAsync();
        }

        public async Task<int> GetCompletedApplicationsCountAsync()
        {
            return await _context.rimsDocumentApplication
                .Where(da => da.Status == "Completed")
                .CountAsync();
        }

        public async Task<DocumentStatistics> GetDocumentStatisticsAsync()
        {
            var applications = await _context.rimsDocumentApplication
                .Include(da => da.Document)
                .ToListAsync();

            var applicationsByType = applications
                .Where(da => da.Document?.DocumentName != null)
                .GroupBy(da => da.Document!.DocumentName) // Using ! because we filtered nulls above
                .ToDictionary(g => g.Key ?? "Unknown", g => g.Count());

            return new DocumentStatistics
            {
                TotalDocuments = await _context.rimsDocuments.CountAsync(),
                PendingApplications = applications.Count(da => da.Status == "Pending"),
                CompletedApplications = applications.Count(da => da.Status == "Completed"),
                RejectedApplications = applications.Count(da => da.Status == "Rejected"),
                ApplicationsByType = applicationsByType
            };
        }

        public async Task<CompletionStatistics> GetCompletionStatisticsAsync()
        {
            var applications = await _context.rimsDocumentApplication.ToListAsync();
            var today = DateTime.Today;
            var weekStart = today.AddDays(-(int)today.DayOfWeek);
            var monthStart = new DateTime(today.Year, today.Month, 1);

            var completedToday = applications.Count(da =>
                da.Status == "Completed" &&
                da.DateInsurance.HasValue &&
                da.DateInsurance.Value.Date == today);

            var completedThisWeek = applications.Count(da =>
                da.Status == "Completed" &&
                da.DateInsurance.HasValue &&
                da.DateInsurance.Value.Date >= weekStart &&
                da.DateInsurance.Value.Date <= today);

            var completedThisMonth = applications.Count(da =>
                da.Status == "Completed" &&
                da.DateInsurance.HasValue &&
                da.DateInsurance.Value.Date >= monthStart &&
                da.DateInsurance.Value.Date <= today);

            var totalApplications = applications.Count;
            var completionRate = totalApplications > 0 ? (decimal)completedThisMonth / totalApplications * 100 : 0;

            var dailyCompletions = applications
                .Where(da => da.Status == "Completed" && da.DateInsurance.HasValue)
                .Select(da => new { Date = da.DateInsurance!.Value.Date }) // Using ! because we filtered HasValue
                .GroupBy(x => x.Date.ToString("MMM dd"))
                .ToDictionary(g => g.Key, g => g.Count());

            return new CompletionStatistics
            {
                TotalApplications = totalApplications,
                CompletedToday = completedToday,
                CompletedThisWeek = completedThisWeek,
                CompletedThisMonth = completedThisMonth,
                CompletionRate = completionRate,
                DailyCompletions = dailyCompletions
            };
        }

        // ID Card operations
        public async Task<int> CreateIDApplicationAsync(IDRequest request)
        {
            var idDocument = await GetOrCreateIDDocumentAsync();
            var application = new RIMSDocumentApplication
            {
                FK_ResidentId = request.ResidentId,
                FK_DocumentId = idDocument,
                Status = "Pending",
                DateInsurance = DateTime.Now,
                ApplicationDate = DateTime.Now,
                Purpose = request.Purpose ?? string.Empty
            };

            _context.rimsDocumentApplication.Add(application);
            await _context.SaveChangesAsync();
            return application.Id;
        }

        public async Task<IEnumerable<IDApplication>> GetIDApplicationsAsync()
        {
            var idDocument = await _context.rimsDocuments
                .FirstOrDefaultAsync(d => d.DocumentName != null && d.DocumentName.Contains("ID Card"));

            if (idDocument == null)
                return Enumerable.Empty<IDApplication>();

            var applications = await _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Where(da => da.FK_DocumentId == idDocument.Id)
                .Select(da => new IDApplication
                {
                    Id = da.Id,
                    ResidentName = $"{da.Resident!.FirstName ?? ""} {da.Resident!.LastName ?? ""}".Trim(), // Using ! because Include ensures Resident is loaded
                    Status = da.Status ?? "Unknown",
                    ApplicationDate = da.DateInsurance ?? DateTime.MinValue
                })
                .ToListAsync();

            return applications;
        }

        public async Task<bool> ApproveIDApplicationAsync(int applicationId)
        {
            var application = await _context.rimsDocumentApplication.FindAsync(applicationId);
            if (application != null)
            {
                application.Status = "Approved";
                application.ApprovedDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<byte[]> GenerateIDCardAsync(int residentId)
        {
            // Implement ID card generation logic
            await Task.Delay(100);
            return Array.Empty<byte>();
        }

        public async Task<IDStatus> GetIDStatusAsync(int residentId)
        {
            var idDocument = await _context.rimsDocuments
                .FirstOrDefaultAsync(d => d.DocumentName != null && d.DocumentName.Contains("ID Card"));

            if (idDocument == null)
                return new IDStatus { HasID = false, Status = "Not Applied" };

            var application = await _context.rimsDocumentApplication
                .FirstOrDefaultAsync(da => da.FK_ResidentId == residentId && da.FK_DocumentId == idDocument.Id);

            return new IDStatus
            {
                HasID = application?.Status == "Approved",
                Status = application?.Status ?? "Not Applied"
            };
        }

        // Certificate generation
        public async Task<byte[]> GenerateCertificatePdfAsync(int applicationId)
        {
            // Implement PDF generation logic
            await Task.Delay(100);
            return Array.Empty<byte>();
        }

        // Helper method
        private async Task<int> GetOrCreateIDDocumentAsync()
        {
            var idDocument = await _context.rimsDocuments
                .FirstOrDefaultAsync(d => d.DocumentName != null && d.DocumentName.Contains("ID Card"));

            if (idDocument == null)
            {
                idDocument = new RIMSDocument
                {
                    DocumentName = "ID Card",
                    DocumentCode = "IDCARD",
                    Description = "Resident Identification Card"
                };
                _context.rimsDocuments.Add(idDocument);
                await _context.SaveChangesAsync();
            }

            return idDocument.Id;
        }
    }
}