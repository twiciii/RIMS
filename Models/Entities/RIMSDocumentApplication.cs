using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models.Entities
{
    [Table("rimsDocumentApplication")]
    public class RIMSDocumentApplication
    {
        [Key]
        [Column("ApplicationId")]
        public int Id { get; set; }

        // Personal Information
        [Required]
        [StringLength(50)]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string CivilStatus { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string ResidencyStatus { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Religion { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Relationship { get; set; }

        [Required]
        [StringLength(50)]
        public string Occupation { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string EducationalAttainment { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string EmploymentStatus { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string MonthlyIncome { get; set; } = string.Empty;

        [Required]
        [StringLength(11)]
        public string ContactNumber { get; set; } = string.Empty;

        // Voting Information
        [StringLength(50)]
        public string? PrecinctNo { get; set; }

        [StringLength(255)]
        public string? PollingPlace { get; set; }

        // Insurance Information
        public DateTime? DateInsurance { get; set; }
        public DateTime? PeriodOfValidity { get; set; }
        public DateTime? ExpirationDate { get; set; }

        [StringLength(50)]
        public string? OfficeHotline { get; set; }

        // Foreign Keys
        [Required]
        [Column("FK_ResidentId")]
        public int FK_ResidentId { get; set; }

        [Required]
        [Column("FK_DocumentId")]
        public int FK_DocumentId { get; set; }

        // Application Details
        [StringLength(255)]
        public string? Purpose { get; set; }

        public int? AddressID { get; set; }

        // Application Status and Tracking
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        // Application Dates and Numbers
        [Required]
        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string ApplicationNumber { get; set; } = string.Empty;

        // Audit Fields
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [StringLength(450)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? ModifiedDate { get; set; }

        [StringLength(450)]
        public string? ModifiedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        [StringLength(450)]
        public string? ApprovedBy { get; set; }

        public DateTime? RejectedDate { get; set; }

        [StringLength(450)]
        public string? RejectedBy { get; set; }

        [StringLength(500)]
        public string? RejectionRemarks { get; set; }

        [StringLength(500)]
        public string? Feedback { get; set; }

        // Navigation properties
        [ForeignKey("FK_ResidentId")]
        public virtual RIMSResident? Resident { get; set; }

        [ForeignKey("FK_DocumentId")]
        public virtual RIMSDocument? Document { get; set; }

        [ForeignKey("AddressID")]
        public virtual RIMSAddress? Address { get; set; }

        // Computed properties for business logic
        [NotMapped]
        public bool IsOverdue => Status == "In Progress" && EstimatedCompletionDate.HasValue && EstimatedCompletionDate.Value < DateTime.Now;

        [NotMapped]
        public string StatusBadgeClass => Status switch
        {
            "Pending" => "bg-warning",
            "Approved" => "bg-success",
            "Rejected" => "bg-danger",
            "In Progress" => "bg-info",
            "Completed" => "bg-success",
            _ => "bg-secondary"
        };

        [NotMapped]
        public int ProcessingDays => (DateTime.Now - ApplicationDate).Days;

        [NotMapped]
        public string ResidentName => Resident?.FullName ?? "N/A";

        [NotMapped]
        public string DocumentName => Document?.DocumentName ?? "N/A";

        [NotMapped]
        public string ContactDisplay => !string.IsNullOrEmpty(ContactNumber) ? ContactNumber :
                                      !string.IsNullOrEmpty(EmailAddress) ? EmailAddress : "N/A";

        [NotMapped]
        public DateTime? EstimatedCompletionDate => ApplicationDate.AddDays(7); // Default 7 days processing
    }
}