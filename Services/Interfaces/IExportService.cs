using RIMS.Models;

namespace RIMS.Services
{
    public interface IExportService
    {
        Task<byte[]> ExportResidentsToExcelAsync(ExportParameters parameters);
        Task<byte[]> ExportResidentsToPdfAsync(ExportParameters parameters);
        Task<byte[]> ExportDocumentsToExcelAsync(ExportParameters parameters);
        Task<byte[]> ExportApplicationsToExcelAsync(ExportParameters parameters);
        Task<byte[]> ExportAuditLogsToExcelAsync(ExportParameters parameters);

        Task<byte[]> ExportToCsvAsync<T>(IEnumerable<T> data, string fileName);
        Task<byte[]> ExportToJsonAsync<T>(IEnumerable<T> data, string fileName);

        Task<string> GenerateExportTemplateAsync(string templateType);
        Task<ExportHistory> LogExportAsync(ExportLog log);
        Task<IEnumerable<ExportLog>> GetExportHistoryAsync();
    }

    public class ExportParameters
    {
        public IEnumerable<int> Ids { get; set; } = new List<int>();
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Format { get; set; } = string.Empty;
        public Dictionary<string, object> Filters { get; set; } = new Dictionary<string, object>();
        public string[] Columns { get; set; } = Array.Empty<string>();
    }

    public class ExportLog
    {
        public int Id { get; set; }
        public string ExportType { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
        public int RecordCount { get; set; }
        public string ExportedBy { get; set; } = string.Empty;
        public DateTime ExportDate { get; set; }
        public string FileName { get; set; } = string.Empty;
    }

    public class ExportHistory
    {
        public int TotalExports { get; set; }
        public IEnumerable<ExportLog> RecentExports { get; set; } = new List<ExportLog>();
        public Dictionary<string, int> ExportsByType { get; set; } = new Dictionary<string, int>();
    }
}