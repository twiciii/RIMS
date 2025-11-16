using System;

namespace RIMS.Models.ViewModels
{
    public class IssuedDocumentViewModel
    {
        public int Id { get; set; }
        public string ResidentName { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string ControlNumber { get; set; } = string.Empty;
        public string ReferenceNumber { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string IssuedBy { get; set; } = string.Empty;
        public string VerifiedBy { get; set; } = string.Empty;
        public decimal FeePaid { get; set; }
        public string PaymentReference { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
    }

    public class IssueDocumentRequestViewModel
    {
        public int ApplicationId { get; set; }
        public string ControlNumber { get; set; } = string.Empty;
        public string ReferenceNumber { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public string IssuedBy { get; set; } = string.Empty;
        public string VerifiedBy { get; set; } = string.Empty;
        public decimal FeePaid { get; set; }
        public string PaymentReference { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
    }
}