namespace RIMS.Configurations
{
    public class ApplicationConfig
    {
        public string ApplicationName { get; set; } = "RIMS";
        public string Version { get; set; } = "1.0.0";
        public string BarangayName { get; set; } = "Sample Barangay";
        public string BarangayCaptain { get; set; } = "Hon. Juan Dela Cruz";
        public string BarangayAddress { get; set; } = "123 Barangay Street, Sample City";
        public string ContactNumber { get; set; } = "(02) 8123-4567";
        public string Email { get; set; } = "barangay@sample.ph";
        public int MaxFileSizeMB { get; set; } = 10;
        public int ReportCacheMinutes { get; set; } = 60;
        public bool EnableAuditLogging { get; set; } = true;
    }
}