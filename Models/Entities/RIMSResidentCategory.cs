using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models.Entities
{
    [Table("rimsResidentCategories")]
    public class RIMSResidentCategory
    {
        [Key]
        [Column("CategoryId")]
        public int Id { get; set; }

        // Add this property to match the column name
        public int CategoryId => Id;

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        public bool IsInformalSettler { get; set; }

        [Required]
        public bool IsPWD { get; set; }

        [Required]
        public bool Is4PsMember { get; set; }

        [Required]
        public bool IsSeniorCitizen { get; set; }

        [Required]
        public bool IsSoloParent { get; set; }

        [Required]
        public bool IsFAR { get; set; }

        [Required]
        public bool IsMAR { get; set; }

        [StringLength(50)]
        public string? Disability { get; set; } // Made nullable

        [StringLength(50)]
        public string? FinancialAid { get; set; } // Made nullable

        [StringLength(50)]
        public string? MedicalAid { get; set; } // Made nullable

        public bool? RequiresSpecification { get; set; }

        // Foreign keys
        [Column("FK_ResidentId")]
        public int ResidentId { get; set; }

        // Navigation properties
        [ForeignKey("ResidentId")]
        public virtual RIMSResident Resident { get; set; } = null!;
    }
}