namespace RIMS.Models.ViewModels
{
    public class AuditFilter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? UserId { get; set; }
        public string? ActionType { get; set; }
        public string? ModuleName { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }

    public class AuditStatistics
    {
        public int TotalLogs { get; set; }
        public int TodayLogs { get; set; }
        public Dictionary<string, int> LogsByActionType { get; set; } = new();
        public Dictionary<string, int> LogsByUser { get; set; } = new();
        public Dictionary<string, int> LogsByModule { get; set; } = new();
        public int ErrorCount { get; set; }
        public int SuccessCount { get; set; }
    }

    public class ExportRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Format { get; set; } = "excel";
    }

    public class AuditLogDto
    {
        public long AuditId { get; set; }
        public string ActionType { get; set; } = string.Empty;
        public string? ModuleName { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; }
        public string? ActionStatus { get; set; }
        public string? Remarks { get; set; }
        public string? IpAddress { get; set; }
    }
}