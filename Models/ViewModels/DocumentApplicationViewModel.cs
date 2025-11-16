using System;

namespace RIMS.Models.ViewModels
{
    public class DocumentApplicationViewModel
    {
        public int Id { get; set; }
        public string ApplicationNumber { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public int DocumentId { get; set; }
        public string ResidentName { get; set; } = string.Empty;
        public int ResidentId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public DateTime? EstimatedCompletionDate { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public string RequirementsSubmitted { get; set; } = string.Empty;
        public decimal? FeeAmount { get; set; }
        public bool IsPaid { get; set; }
        public string PaymentReference { get; set; } = string.Empty;
        public string ProcessedBy { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public bool IsUrgent { get; set; }
        public string UrgentReason { get; set; } = string.Empty;

        // For display purposes
        public string StatusBadgeClass => Status switch
        {
            "Pending" => "bg-warning",
            "Approved" => "bg-success",
            "Rejected" => "bg-danger",
            "In Progress" => "bg-info",
            "Completed" => "bg-success",
            _ => "bg-secondary"
        };
    }
}