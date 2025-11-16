using RIMS.Models.Entities;
using RIMS.Models.ViewModels;

namespace RIMS.Services.Interfaces
{
    public interface IDocumentService
    {
        // Document operations
        Task<RIMSDocument> GetDocumentByIdAsync(int id);
        Task<IEnumerable<RIMSDocument>> GetAllDocumentsAsync();
        Task<RIMSDocument> CreateDocumentAsync(RIMSDocument document);
        Task<RIMSDocument> UpdateDocumentAsync(RIMSDocument document);
        Task<bool> DeleteDocumentAsync(int id);

        // Document Application operations
        Task<RIMSDocumentApplication> GetDocumentApplicationByIdAsync(int id);
        Task<IEnumerable<RIMSDocumentApplication>> GetAllDocumentApplicationsAsync();
        Task<IEnumerable<RIMSDocumentApplication>> GetPendingApplicationsAsync();
        Task<IEnumerable<RIMSDocumentApplication>> GetCompletedApplicationsAsync();
        Task<IEnumerable<RIMSDocumentApplication>> GetCertificateApplicationsAsync();
        Task<IEnumerable<RIMSDocumentApplication>> GetPendingCertificateApplicationsAsync();
        Task<IEnumerable<RIMSDocumentApplication>> GetTodayCompletedApplicationsAsync(); // Added missing method
        Task<IEnumerable<RIMSDocumentApplication>> GetCompletedApplicationsByDateAsync(DateTime date); // Added missing method
        Task<RIMSDocumentApplication> CreateDocumentApplicationAsync(RIMSDocumentApplication application);
        Task<RIMSDocumentApplication> UpdateDocumentApplicationAsync(RIMSDocumentApplication application);
        Task<bool> DeleteDocumentApplicationAsync(int id);
        Task<bool> ArchiveApplicationAsync(int id); // Added missing method

        // Application processing
        Task<bool> ApproveApplicationAsync(int applicationId);
        Task<bool> RejectApplicationAsync(int applicationId, string remarks);
        Task<bool> RejectCertificateAsync(int applicationId, string remarks);
        Task<bool> ReturnApplicationAsync(int applicationId, string feedback);
        Task<bool> IssueCertificateAsync(int applicationId);

        // Search and filtering
        Task<IEnumerable<RIMSDocumentApplication>> SearchDocumentApplicationsAsync(string searchTerm);
        Task<IEnumerable<RIMSDocumentApplication>> SearchByDocumentTypeAsync(string documentType);
        Task<IEnumerable<RIMSDocumentApplication>> SearchByDateRangeAsync(DateTime fromDate, DateTime toDate);

        // Statistics
        Task<int> GetPendingApplicationsCountAsync();
        Task<int> GetCompletedApplicationsCountAsync();
        Task<DocumentStatistics> GetDocumentStatisticsAsync();
        Task<CompletionStatistics> GetCompletionStatisticsAsync(); // Added missing method

        // ID Card operations
        Task<int> CreateIDApplicationAsync(IDRequest request);
        Task<IEnumerable<IDApplication>> GetIDApplicationsAsync();
        Task<bool> ApproveIDApplicationAsync(int applicationId);
        Task<byte[]> GenerateIDCardAsync(int residentId);
        Task<IDStatus> GetIDStatusAsync(int residentId);

        // Certificate generation
        Task<byte[]> GenerateCertificatePdfAsync(int applicationId);
    }

    public class DocumentStatistics
    {
        public int TotalDocuments { get; set; }
        public int PendingApplications { get; set; }
        public int CompletedApplications { get; set; }
        public int RejectedApplications { get; set; }
        public Dictionary<string, int> ApplicationsByType { get; set; } = new();
    }

    // Add these supporting classes to resolve the namespace conflicts
    public class IDRequest
    {
        public int ResidentId { get; set; }
        public string? Purpose { get; set; }
    }

    public class IDApplication
    {
        public int Id { get; set; }
        public string ResidentName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; }
    }

    public class IDStatus
    {
        public bool HasID { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CompletionStatistics
    {
        public int TotalApplications { get; set; }
        public int CompletedToday { get; set; }
        public int CompletedThisWeek { get; set; }
        public int CompletedThisMonth { get; set; }
        public decimal CompletionRate { get; set; }
        public Dictionary<string, int> DailyCompletions { get; set; } = new();
    }
}