using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models.Entities
{
    [Table("rimsStreets")]
    public class RIMSStreets
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Highway { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Oneway { get; set; } = string.Empty;

        [StringLength(100)]
        public string OldName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string StreetId { get; set; } = string.Empty;

        [Required]
        public string Geometry { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<RIMSAddress> Addresses { get; set; } = new List<RIMSAddress>();
    }
}