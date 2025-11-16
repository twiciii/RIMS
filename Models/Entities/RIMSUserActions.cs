using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models.Entities
{
    [Table("rimsUserActions")]
    public class RIMSUserActions
    {
        [Key]
        [Column("ActionID")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ActionLabel { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public virtual ICollection<RIMSAuditTrail> AuditTrails { get; set; } = new List<RIMSAuditTrail>();
    }
}