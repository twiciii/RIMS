using RIMS.Models.Entities;
using RIMS.Models.ViewModels;

namespace RIMS.Services
{
    public interface ISystemService
    {
        // System configuration
        Task<SystemConfiguration> GetSystemConfigurationAsync();
        Task UpdateSystemConfigurationAsync(SystemConfiguration configuration);
        Task<Dictionary<string, string>> GetSystemSettingsAsync();
        Task UpdateSystemSettingAsync(string key, string value);

        // Maintenance operations
        Task<SystemHealth> GetSystemHealthAsync();
        Task RunDatabaseMaintenanceAsync();
        Task CleanupTempFilesAsync();
        Task BackupDatabaseAsync(string backupPath);
        Task RestoreDatabaseAsync(string backupPath);

        // User management
        Task<IEnumerable<RIMSUsers>> GetSystemUsersAsync();
        Task<RIMSUsers> CreateUserAsync(RIMSUsers user, string password);
        Task<RIMSUsers> UpdateUserAsync(RIMSUsers user);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> ResetPasswordAsync(string userId, string newPassword);

        // System logs
        Task<IEnumerable<SystemLog>> GetSystemLogsAsync(LogFilter filter);
        Task ClearSystemLogsAsync();

        // Performance monitoring
        Task<SystemPerformance> GetSystemPerformanceAsync();
        Task<ResourceUsage> GetResourceUsageAsync();
    }

    public class SystemConfiguration
    {
        public string SystemName { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string SupportPhone { get; set; } = string.Empty;
        public int SessionTimeout { get; set; }
        public int MaxLoginAttempts { get; set; }
        public bool MaintenanceMode { get; set; }
        public string[] EnabledModules { get; set; } = Array.Empty<string>();
    }

    public class SystemHealth
    {
        public bool DatabaseConnected { get; set; }
        public bool EmailServiceWorking { get; set; }
        public bool FileSystemAccessible { get; set; }
        public DateTime LastBackup { get; set; }
        public Dictionary<string, bool> ServiceStatus { get; set; } = new Dictionary<string, bool>();
    }

    public class SystemLog
    {
        public int Id { get; set; }
        public string Level { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Exception { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; } = string.Empty;
    }

    public class LogFilter
    {
        public string Level { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string UserId { get; set; } = string.Empty;
    }

    public class ResourceUsage
    {
        public double CpuUsage { get; set; }
        public double MemoryUsage { get; set; }
        public double DiskUsage { get; set; }
        public int ActiveConnections { get; set; }
    }

    // Remove the duplicate SystemPerformance class from here
    // It should only exist in RIMS.Models.ViewModels namespace
}