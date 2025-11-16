// IndigencyModel.cs
using RIMS.Models.Certificates;
using System.ComponentModel.DataAnnotations;

public class IndigencyModel : BaseCertificateModel
{
    [Required(ErrorMessage = "Monthly income range is required")]
    [StringLength(20)]
    public string MonthlyIncome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Employment status is required")]
    [StringLength(50)]
    public string EmploymentStatus { get; set; } = string.Empty;

    [Required(ErrorMessage = "Educational attainment is required")]
    [StringLength(50)]
    public string EducationalAttainment { get; set; } = string.Empty;

    public string CertificateType => "Certificate of Indigency";
}