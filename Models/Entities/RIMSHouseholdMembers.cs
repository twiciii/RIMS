using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models.Entities
{
    [Table("rimsHouseholdMembers")]
    public class RIMSHouseholdMembers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ResidentId { get; set; }

        [Required]
        public int HeadOfHouseholdId { get; set; }

        // Member personal information
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(10)]
        public string? Suffix { get; set; }

        // Household relationship information
        [Required]
        [StringLength(100)]
        public string Relationship { get; set; } = string.Empty;

        [Required]
        public bool IsHeadOfHousehold { get; set; }

        public int NumberOfDependents { get; set; }

        // Audit fields
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        [ForeignKey("ResidentId")]
        public virtual RIMSResident? Resident { get; set; }

        [ForeignKey("HeadOfHouseholdId")]
        public virtual RIMSResident? HeadOfHousehold { get; set; }

        // Computed property for full name
        [NotMapped]
        public string FullName
        {
            get
            {
                var names = new List<string> { FirstName };
                if (!string.IsNullOrEmpty(MiddleName))
                    names.Add(MiddleName);
                names.Add(LastName);
                if (!string.IsNullOrEmpty(Suffix))
                    names.Add(Suffix);

                return string.Join(" ", names);
            }
        }
    }
}