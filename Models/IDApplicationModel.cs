using System;
using System.ComponentModel.DataAnnotations;
using RIMS.Models;

namespace RIMS.Models
{
    public class IDApplicationModel : PersonInfoModel
    {
        [Required(ErrorMessage = "Civil status is required")]
        [Display(Name = "Civil Status")]
        public string CivilStatus { get; set; } = string.Empty;

        [Required(ErrorMessage = "House number is required")]
        [Display(Name = "House No.")]
        [StringLength(20, ErrorMessage = "House number cannot exceed 20 characters")]
        public string HouseNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Street name is required")]
        [Display(Name = "Street Name")]
        [StringLength(100, ErrorMessage = "Street name cannot exceed 100 characters")]
        public string StreetName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Purok is required")]
        [Display(Name = "Purok")]
        [StringLength(50, ErrorMessage = "Purok cannot exceed 50 characters")]
        public string Purok { get; set; } = string.Empty;

        [Required(ErrorMessage = "Years of residence is required")]
        [Display(Name = "Years of Residence")]
        [Range(1, 100, ErrorMessage = "Years of residence must be between 1 and 100")]
        public int YearsOfResidence { get; set; } = 1;

        // Emergency Contact
        [Required(ErrorMessage = "Emergency contact name is required")]
        [Display(Name = "Emergency Contact Name")]
        [StringLength(100, ErrorMessage = "Emergency contact name cannot exceed 100 characters")]
        public string EmergencyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Emergency contact number is required")]
        [Display(Name = "Emergency Contact Number")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(15, ErrorMessage = "Emergency contact number cannot exceed 15 characters")]
        public string EmergencyContact { get; set; } = string.Empty;

        [Required(ErrorMessage = "Relationship is required")]
        [Display(Name = "Relationship")]
        [StringLength(50, ErrorMessage = "Relationship cannot exceed 50 characters")]
        public string EmergencyRelationship { get; set; } = string.Empty;

        // Photo
        [Display(Name = "Photo Data")]
        public string PhotoData { get; set; } = string.Empty; // Base64 string for captured photo

        [Display(Name = "Signature")]
        public string Signature { get; set; } = string.Empty;

        // Application metadata
        [Display(Name = "Application Date")]
        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        [Display(Name = "Application ID")]
        public string ApplicationId { get; set; } = string.Empty;

        [Display(Name = "Status")]
        public string Status { get; set; } = "Pending";

        // Computed properties
        public string FullAddress => $"{HouseNo} {StreetName}, Purok {Purok}, Barangay San Bartolome, Quezon City";

        public string IDNumber => GenerateIDNumber();

        public DateTime ExpiryDate => ApplicationDate.AddYears(3);

        private string GenerateIDNumber()
        {
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var random = new Random().Next(1000, 9999);
            return $"BID-{timestamp}-{random}";
        }
    }
}