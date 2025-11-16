using System.ComponentModel.DataAnnotations;

namespace RIMS.Models.ViewModels
{
    public class ResidentCreateDto
    {
        // Personal Information
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Middle name cannot exceed 50 characters")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(10, ErrorMessage = "Suffix cannot exceed 10 characters")]
        public string? Suffix { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Place of birth is required")]
        [StringLength(100, ErrorMessage = "Place of birth cannot exceed 100 characters")]
        public string PlaceOfBirth { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sex is required")]
        public string Sex { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nationality is required")]
        [StringLength(50, ErrorMessage = "Nationality cannot exceed 50 characters")]
        public string Nationality { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(120, ErrorMessage = "Address cannot exceed 120 characters")]
        public string Address { get; set; } = string.Empty;

        // Document Application Information
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters")]
        public string? EmailAddress { get; set; }

        [Required(ErrorMessage = "Civil status is required")]
        public string CivilStatus { get; set; } = string.Empty;

        [Required(ErrorMessage = "Residency status is required")]
        public string ResidencyStatus { get; set; } = string.Empty;

        [Required(ErrorMessage = "Religion is required")]
        public string Religion { get; set; } = string.Empty;

        public string? Relationship { get; set; }

        [Required(ErrorMessage = "Occupation is required")]
        [StringLength(50, ErrorMessage = "Occupation cannot exceed 50 characters")]
        public string Occupation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Educational attainment is required")]
        public string EducationalAttainment { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employment status is required")]
        public string EmploymentStatus { get; set; } = string.Empty;

        [Required(ErrorMessage = "Monthly income is required")]
        public string MonthlyIncome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact number is required")]
        [RegularExpression(@"^09[0-9]{9}$", ErrorMessage = "Contact number must be 11 digits starting with 09")]
        [StringLength(11, ErrorMessage = "Contact number must be 11 digits")]
        public string ContactNumber { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Purpose cannot exceed 255 characters")]
        public string? Purpose { get; set; }

        // Household Information
        public bool IsHeadOfHousehold { get; set; }
        public int? NumberOfDependents { get; set; }
        public int? HeadOfHouseholdId { get; set; }

        // Categories
        public List<CategoryDto> Categories { get; set; } = new();

        // Address Details (for residency)
        public string? LotNo { get; set; }
        public string? BlockNo { get; set; }
        public string? BldgNo { get; set; }
        public string? Purok { get; set; }
        public int? StreetId { get; set; }
    }

    public class ResidentFullDto
    {
        public int ResidentId { get; set; }

        // Personal Information
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? Suffix { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // Household Information
        public bool IsHeadOfHousehold { get; set; }
        public int? NumberOfDependents { get; set; }

        // Document Application Information
        public string? EmailAddress { get; set; }
        public string? CivilStatus { get; set; }
        public string? ResidencyStatus { get; set; }
        public string? Religion { get; set; }
        public string? Relationship { get; set; }
        public string? Occupation { get; set; }
        public string? EducationalAttainment { get; set; }
        public string? EmploymentStatus { get; set; }
        public string? MonthlyIncome { get; set; }
        public string? ContactNumber { get; set; }
        public string? Purpose { get; set; }

        // Categories
        public bool IsPWD { get; set; }
        public bool Is4PsMember { get; set; }
        public bool IsSeniorCitizen { get; set; }
        public bool IsSoloParent { get; set; }
        public bool IsInformalSettler { get; set; }
        public bool IsFAR { get; set; }
        public bool IsMAR { get; set; }
        public string? Disability { get; set; }
        public string? FinancialAid { get; set; }
        public string? MedicalAid { get; set; }

        // Household Members
        public List<HouseholdMemberDto> HouseholdMembers { get; set; } = new();

        // Computed Properties
        public string FullName =>
            $"{FirstName} {MiddleName} {LastName} {Suffix}".Trim().Replace("  ", " ");

        public string ShortName =>
            $"{FirstName} {LastName}".Trim();
    }

    public class HouseholdMemberDto
    {
        public int ResidentId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty;
        public bool IsHeadOfHousehold { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public int HeadOfHouseholdId { get; set; }

        // Computed Properties
        public string FullName =>
            $"{FirstName} {MiddleName} {LastName}".Trim().Replace("  ", " ");
    }

    public class CategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public bool IsPWD { get; set; }
        public bool Is4PsMember { get; set; }
        public bool IsSeniorCitizen { get; set; }
        public bool IsSoloParent { get; set; }
        public bool IsInformalSettler { get; set; }
        public bool IsFAR { get; set; }
        public bool IsMAR { get; set; }
        public string? Disability { get; set; }
        public string? FinancialAid { get; set; }
        public string? MedicalAid { get; set; }
    }

    // Additional DTOs for specific operations
    public class ResidentUpdateDto
    {
        public int ResidentId { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? MiddleName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [StringLength(10)]
        public string? Suffix { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(100)]
        public string? PlaceOfBirth { get; set; }

        public string? Sex { get; set; }

        [StringLength(50)]
        public string? Nationality { get; set; }

        [StringLength(120)]
        public string? Address { get; set; }

        public bool? IsHeadOfHousehold { get; set; }
        public int? NumberOfDependents { get; set; }
    }

    public class ResidentSearchDto
    {
        public string? SearchTerm { get; set; }
        public string? Category { get; set; }
        public string? Purok { get; set; }
        public string? CivilStatus { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public bool? IsHeadOfHousehold { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class ResidentListDto
    {
        public int ResidentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Address { get; set; } = string.Empty;
        public string? ContactNumber { get; set; }
        public string? Occupation { get; set; }
        public bool IsHeadOfHousehold { get; set; }
        public string Categories { get; set; } = string.Empty;
    }
}