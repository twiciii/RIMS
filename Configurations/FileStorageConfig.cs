namespace RIMS.Configurations
{
    public class FileStorageConfig
    {
        public string BasePath { get; set; } = "Uploads";
        public int MaxFileSizeMB { get; set; } = 10;
        public string[] AllowedExtensions { get; set; } = { ".jpg", ".jpeg", ".png", ".pdf" };
        public string BackupPath { get; set; } = "Backups";
        public int CleanupIntervalHours { get; set; } = 24;
        public int BackupRetentionDays { get; set; } = 30;
    }
}