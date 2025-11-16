using System;
using System.ComponentModel.DataAnnotations;
using RIMS.Models.Entities;

namespace RIMS.Models.Certificates
{
    public abstract class BaseCertificateModel
    {
        // Resident Information (matches rimsResidents table)
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Middle name cannot exceed 50 characters")]
        public string? MiddleName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "Suffix cannot exceed 10 characters")]
        public string? Suffix { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Place of birth is required")]
        [StringLength(100, ErrorMessage = "Place of birth cannot exceed 100 characters")]
        public string PlaceOfBirth { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sex is required")]
        [StringLength(10)]
        public string Sex { get; set; } = string.Empty;

        // Additional information for document application
        [Required(ErrorMessage = "Civil status is required")]
        [StringLength(20)]
        public string CivilStatus { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact number is required")]
        [StringLength(11)]
        [RegularExpression(@"^09[0-9]{9}$", ErrorMessage = "Contact number must start with 09 and be 11 digits long")]
        public string ContactNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Occupation is required")]
        [StringLength(50)]
        public string Occupation { get; set; } = string.Empty;

        // Address information (matches rimsAddresses structure)
        [Required(ErrorMessage = "House/Lot number is required")]
        [StringLength(50)]
        public string LotNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Block number is required")]
        [StringLength(50)]
        public string BlockNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Building number is required")]
        [StringLength(50)]
        public string BldgNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Purok is required")]
        [StringLength(100)]
        public string Purok { get; set; } = string.Empty;

        [Required(ErrorMessage = "Purpose is required")]
        [StringLength(255)]
        public string Purpose { get; set; } = string.Empty;

        // Computed properties
        public string FullName => $"{FirstName} {MiddleName} {LastName} {Suffix}".Replace("  ", " ").Trim();
        public int Age => DateTime.Now.Year - DateOfBirth.Year - (DateTime.Now.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
        public string FullAddress => $"{LotNo} {BlockNo} {BldgNo}, Purok {Purok}".Trim();
    }
}