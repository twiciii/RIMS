using System;

namespace RIMS.Models.ViewModels
{
    public class ApplicationSummaryViewModel
    {
        public int Id { get; set; }
        public string ApplicationNumber { get; set; } = string.Empty;
        public string ResidentName { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public DateTime? EstimatedCompletionDate { get; set; }
        public string ProcessedBy { get; set; } = string.Empty;
        public bool IsUrgent { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }
}