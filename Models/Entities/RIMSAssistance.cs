using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models.Entities
{
    [Table("rimsAssistance")]
    public class RIMSAssistance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Type { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        // Foreign keys
        [Required]
        public int ResidentId { get; set; }

        // Navigation properties
        [ForeignKey("ResidentId")]
        public virtual RIMSResident Resident { get; set; } = null!;
    }
}