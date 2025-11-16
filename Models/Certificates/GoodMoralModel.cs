// GoodMoralModel.cs
using RIMS.Models.Certificates;
using System.ComponentModel.DataAnnotations;

public class GoodMoralModel : BaseCertificateModel
{
    [Required(ErrorMessage = "Educational institution is required")]
    [StringLength(100)]
    public string EducationalInstitution { get; set; } = string.Empty;

    public bool HasCriminalRecord { get; set; } = false;

    public string CertificateType => "Good Moral Character Certificate";
}