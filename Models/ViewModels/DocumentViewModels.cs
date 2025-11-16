using System;
using System.Collections.Generic;

namespace RIMS.Models.ViewModels
{
    public class DocumentViewModel
    {
        public int Id { get; set; }
        public string DocumentName { get; set; } = string.Empty;
        public string DocumentCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
        public decimal? Fee { get; set; }
        public int ProcessingDays { get; set; }
        public int ValidityDays { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;

        // Statistics
        public int TotalApplications { get; set; }
        public int PendingApplications { get; set; }
        public decimal CompletionRate { get; set; }
    }

    public class DocumentCreateViewModel
    {
        public string DocumentName { get; set; } = string.Empty;
        public string DocumentCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
        public decimal? Fee { get; set; }
        public int ProcessingDays { get; set; }
        public int ValidityDays { get; set; }
        public bool IsActive { get; set; } = true;
    }
}