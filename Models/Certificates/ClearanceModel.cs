// ClearanceModel.cs
using RIMS.Models.Certificates;
using System.ComponentModel.DataAnnotations;

public class ClearanceModel : BaseCertificateModel
{
    public bool HasPendingCase { get; set; } = false;

    [StringLength(500)]
    public string PreviousViolations { get; set; } = string.Empty;

    public string CertificateType => "Barangay Clearance";
}