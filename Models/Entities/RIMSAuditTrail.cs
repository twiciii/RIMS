using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models.Entities
{
    [Table("rimsAuditTrail")]
    public class RIMSAuditTrail
    {
        [Key]
        [Column("AuditId")]
        public int? Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ActionType { get; set; } = string.Empty;

        [StringLength(50)]
        public string? ModuleName { get; set; }

        [StringLength(50)]
        public string? TableName { get; set; } // Added missing property

        public int? ApplicationId { get; set; }

        [Required]
        [StringLength(100)]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public DateTime ActionDate { get; set; }

        public DateTime? LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }

        [StringLength(50)]
        public string? IpAddress { get; set; }

        [StringLength(256)]
        public string? UserAgent { get; set; }

        [StringLength(20)]
        public string? ActionStatus { get; set; }

        public string? ErrorMessage { get; set; }

        [StringLength(50)]
        public string? TransactionId { get; set; }

        [StringLength(500)]
        public string? Remarks { get; set; }

        public bool? IsDeleted { get; set; }

        public int? ActionId { get; set; }

        // Added missing properties for change tracking
        public string? KeyValues { get; set; } // Added missing property
        public string? OldValues { get; set; } // Added missing property  
        public string? NewValues { get; set; } // Added missing property

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual RIMSUsers? User { get; set; }

        [ForeignKey("ActionId")]
        public virtual RIMSUserActions? UserAction { get; set; }
    }
}