using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIMS.Models.Entities
{
    [Table("rimsResidents")]
    public class RIMSResident
    {
        [Key]
        [Column("ResidentId")]
        public int Id { get; set; }

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

        // Computed properties for display
        [NotMapped]
        public string FullName
        {
            get
            {
                var name = $"{FirstName} {LastName}";
                if (!string.IsNullOrEmpty(Suffix))
                    name += $" {Suffix}";
                return name;
            }
        }

        [NotMapped]
        public string FullNameWithMiddle
        {
            get
            {
                var name = $"{FirstName}";
                if (!string.IsNullOrEmpty(MiddleName))
                    name += $" {MiddleName}";
                name += $" {LastName}";
                if (!string.IsNullOrEmpty(Suffix))
                    name += $" {Suffix}";
                return name;
            }
        }

        // Personal Information
        [Required]
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(100)]
        public string PlaceOfBirth { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Sex { get; set; } = string.Empty;

        // Age is computed, not stored - kept as NotMapped based on second version
        [NotMapped]
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Nationality { get; set; } = string.Empty;

        // Contact Information
        [StringLength(120)]
        public string? Address { get; set; }

        [StringLength(20)]
        public string? ContactNumber { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? CivilStatus { get; set; }

        // Additional Information
        [StringLength(100)]
        public string? Occupation { get; set; }

        // System Fields
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        [StringLength(100)]
        public string? ModifiedBy { get; set; }

        // Household Head Information
        public bool IsHeadOfHousehold { get; set; }
        public int? NumberOfDependents { get; set; }

        // Self-referencing for household relationships
        public int? HeadOfHouseholdId { get; set; }

        [ForeignKey("HeadOfHouseholdId")]
        public virtual RIMSResident? HeadOfHousehold { get; set; }

        public virtual ICollection<RIMSResident> Dependents { get; set; } = new List<RIMSResident>();

        // Navigation properties
        public virtual ICollection<RIMSResidentCategory> ResidentCategories { get; set; } = new List<RIMSResidentCategory>();
        public virtual ICollection<RIMSDocumentApplication> DocumentApplications { get; set; } = new List<RIMSDocumentApplication>();
        public virtual ICollection<RIMSHouseholdMembers> HouseholdMembers { get; set; } = new List<RIMSHouseholdMembers>(); // ADDED
    }
}