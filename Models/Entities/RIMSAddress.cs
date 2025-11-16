using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace RIMS.Models.Entities
{
    [Table("rimsAddresses")]
    public class RIMSAddress
    {
        [Key]
        [Column("AddressId")]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string LotNo { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string BlockNo { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string BldgNo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Purok { get; set; } = string.Empty;

        // Foreign key
        [Column("FK_Id")]
        public int StreetId { get; set; }

        // Navigation property
        [ForeignKey("StreetId")]
        public virtual RIMSStreets Street { get; set; } = null!;

        public virtual ICollection<RIMSDocumentApplication> DocumentApplications { get; set; } = new List<RIMSDocumentApplication>();
    }
}