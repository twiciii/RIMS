using RIMS.Models.Entities;

namespace RIMS.Models.ViewModels
{
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

    public class SystemPerformance
    {
        public TimeSpan Uptime { get; set; }
        public double AverageResponseTime { get; set; }
        public double ErrorRate { get; set; }
        public int ActiveUsers { get; set; }
        public string DatabasePerformance { get; set; } = string.Empty;
        public DateTime LastMaintenance { get; set; }
    }

    public class ResourceUsage
    {
        public double CpuUsage { get; set; }
        public double MemoryUsage { get; set; }
        public double DiskUsage { get; set; }
        public int ActiveConnections { get; set; }
    }

    public class SystemLog
    {
        public int Id { get; set; }
        public string Level { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Exception { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; } = string.Empty;
    }

    public class LogFilter
    {
        public string? Level { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? UserId { get; set; }
    }
}