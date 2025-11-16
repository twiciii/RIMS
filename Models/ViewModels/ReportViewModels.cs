using System;
using System.Collections.Generic;

namespace RIMS.Models.ViewModels
{
    public class ReportViewModel
    {
        public string ReportType { get; set; } = string.Empty;
        public string ReportTitle { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public object Data { get; set; } = new();
        public DateTime GeneratedDate { get; set; }
        public string GeneratedBy { get; set; } = string.Empty;
        public string ReportFormat { get; set; } = "PDF"; // PDF, Excel, CSV
        public Dictionary<string, object> Filters { get; set; } = new();
        public ReportSummary Summary { get; set; } = new();
    }

    public class ReportSummary
    {
        public int TotalRecords { get; set; }
        public decimal TotalAmount { get; set; }
        public int CompletedCount { get; set; }
        public int PendingCount { get; set; }
        public Dictionary<string, int> CategoryBreakdown { get; set; } = new();
    }

    public class ReportRequestViewModel
    {
        public string ReportType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Format { get; set; } = "PDF";
        public Dictionary<string, object> Filters { get; set; } = new();
    }
}