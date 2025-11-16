using System;
using System.Collections.Generic;

namespace RIMS.Models.ViewModels
{
    public class ResidentProfileViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string Suffix { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public DateTime BirthDate { get; set; }
        public int Age => DateTime.Now.Year - BirthDate.Year - (DateTime.Now.DayOfYear < BirthDate.DayOfYear ? 1 : 0);
        public string Gender { get; set; } = string.Empty;
        public string CivilStatus { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Address Information
        public string Address { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Barangay { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        // Additional Information
        public string Occupation { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public string BirthPlace { get; set; } = string.Empty;

        // Profile Image
        public string ProfileImagePath { get; set; } = string.Empty;

        // Application History
        public List<DocumentApplicationViewModel> DocumentApplications { get; set; } = new();

        // Statistics
        public int TotalApplications { get; set; }
        public int CompletedApplications { get; set; }
        public int PendingApplications { get; set; }
        public DateTime? LastApplicationDate { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}