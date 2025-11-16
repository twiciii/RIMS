using RIMS.Models.Entities;
using RIMS.Models.ViewModels;

namespace RIMS.Services.Interfaces
{
    public interface IAuditService
    {
        // Basic audit operations
        Task<IEnumerable<RIMSAuditTrail>> GetAuditLogsAsync(AuditFilter filter);
        Task<IEnumerable<RIMSAuditTrail>> GetUserAuditLogsAsync(string userId);
        Task<IEnumerable<RIMSAuditTrail>> GetAuditLogsByDateAsync(DateTime date);
        Task<IEnumerable<RIMSAuditTrail>> GetAuditLogsByActionAsync(string actionType);

        // New methods needed by controller
        Task<IEnumerable<RIMSAuditTrail>> GetRecentAuditLogsAsync();
        Task<int> CleanupOldAuditLogsAsync(int daysToKeep);

        // Statistics and reporting
        Task<AuditStatistics> GetAuditStatisticsAsync(DateTime fromDate, DateTime toDate);
        Task<byte[]> ExportAuditLogsAsync(DateTime fromDate, DateTime toDate, string format = "excel");

        // Logging methods
        Task LogErrorAsync(string action, string errorMessage, string? userId = null);
        Task LogActionAsync(string actionType, string description, string userId); // Fixed signature
        Task LogLoginAsync(string userId, string ipAddress, string userAgent);
        Task LogLogoutAsync(string userId);
    }

    // Move these to separate files or ensure they don't conflict with Models.ViewModels
    public class AuditFilter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? UserId { get; set; }
        public string? ActionType { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }

    public class AuditStatistics
    {
        public int TotalActions { get; set; }
        public int TotalUsers { get; set; }
        public int TotalErrors { get; set; }
        public int TotalLogins { get; set; }
        public Dictionary<string, int> ActionsByType { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ActionsByUser { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ActionsByModule { get; set; } = new Dictionary<string, int>();
    }
}