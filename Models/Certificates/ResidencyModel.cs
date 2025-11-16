// ResidencyModel.cs
using RIMS.Models.Certificates;
using System.ComponentModel.DataAnnotations;

public class ResidencyModel : BaseCertificateModel
{
    [Required(ErrorMessage = "Years of residence is required")]
    [Range(1, 100, ErrorMessage = "Years of residence must be between 1 and 100")]
    public int YearsOfResidence { get; set; } = 1;

    public string CertificateType => "Certificate of Residency";
}

