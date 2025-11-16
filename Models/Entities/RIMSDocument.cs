using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models.Entities
{
    [Table("rimsDocuments")]
    public class RIMSDocument
    {
        [Key]
        [Column("DocumentId")]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string DocumentName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string FilePath { get; set; } = string.Empty;

        // Additional properties for document management
        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(1000)]
        public string? Requirements { get; set; }

        public decimal? Fee { get; set; }

        public int ProcessingDays { get; set; } = 3;

        public int ValidityDays { get; set; } = 30;

        public bool IsActive { get; set; } = true;

        [StringLength(50)]
        public string? DocumentType { get; set; } // Certificate, Clearance, ID, etc.

        [StringLength(50)]
        public string? DocumentCode { get; set; }

        // System fields
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        [StringLength(100)]
        public string? ModifiedBy { get; set; }

        // Navigation property
        public virtual ICollection<RIMSDocumentApplication> DocumentApplications { get; set; } = new List<RIMSDocumentApplication>();

        // Computed properties for business logic
        [NotMapped]
        public bool IsFree => Fee == null || Fee == 0;

        [NotMapped]
        public string StatusBadgeClass => IsActive ? "bg-success" : "bg-secondary";

        [NotMapped]
        public string FeeDisplay => IsFree ? "Free" : $"₱{Fee:N2}";

        [NotMapped]
        public string ProcessingTimeDisplay => ProcessingDays == 1 ? "1 day" : $"{ProcessingDays} days";

        [NotMapped]
        public string ValidityDisplay => ValidityDays == 1 ? "1 day" : $"{ValidityDays} days";
    }
}