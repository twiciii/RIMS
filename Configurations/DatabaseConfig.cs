namespace RIMS.Configurations
{
    public class DatabaseConfig
    {
        public string DefaultConnection { get; set; } = string.Empty;
        public int CommandTimeout { get; set; } = 30;
        public bool EnableSensitiveDataLogging { get; set; } = false;
        public bool EnableDetailedErrors { get; set; } = false;
    }
}