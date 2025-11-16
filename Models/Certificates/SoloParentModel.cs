// SoloParentModel.cs
using RIMS.Models.Certificates;
using System.ComponentModel.DataAnnotations;

public class SoloParentModel : BaseCertificateModel
{
    [Required(ErrorMessage = "Number of children is required")]
    [Range(1, 20, ErrorMessage = "Number of children must be between 1 and 20")]
    public int NumberOfChildren { get; set; } = 1;

    [Required(ErrorMessage = "Reason for solo parenting is required")]
    [StringLength(200)]
    public string Reason { get; set; } = string.Empty;

    public string CertificateType => "Solo Parent Certificate";
}