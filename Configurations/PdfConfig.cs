namespace RIMS.Configurations
{
    public class PdfConfig
    {
        public string DefaultFont { get; set; } = "Arial";
        public int HeaderFontSize { get; set; } = 16;
        public int BodyFontSize { get; set; } = 12;
        public string PageSize { get; set; } = "A4";
        public double MarginInches { get; set; } = 0.5;
        public string WatermarkText { get; set; } = "BARANGAY OFFICIAL DOCUMENT";
        public bool IncludeWatermark { get; set; } = true;
        public string CertificateHeader { get; set; } = "OFFICE OF THE BARANGAY CHAIRMAN";
    }
}