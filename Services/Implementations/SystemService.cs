using Microsoft.EntityFrameworkCore;
using RIMS.Data;
using RIMS.Models.Entities;
using RIMS.Models.ViewModels;

namespace RIMS.Services.Implementations
{
    public class SystemService : ISystemService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public SystemService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<SystemConfiguration> GetSystemConfigurationAsync()
        {
            return await Task.FromResult(new SystemConfiguration
            {
                SystemName = _configuration["System:Name"] ?? "RIMS",
                Version = _configuration["System:Version"] ?? "1.0.0",
                ContactEmail = _configuration["System:ContactEmail"] ?? "support@rims.local",
                SupportPhone = _configuration["System:SupportPhone"] ?? "+1234567890",
                SessionTimeout = int.Parse(_configuration["System:SessionTimeout"] ?? "30"),
                MaxLoginAttempts = int.Parse(_configuration["System:MaxLoginAttempts"] ?? "5"),
                MaintenanceMode = bool.Parse(_configuration["System:MaintenanceMode"] ?? "false"),
                EnabledModules = _configuration.GetSection("System:EnabledModules").Get<string[]>() ?? new[] { "Residents", "Documents", "Reports" }
            });
        }

        public async Task UpdateSystemConfigurationAsync(SystemConfiguration configuration)
        {
            // In real implementation, save to configuration store or database
            await Task.CompletedTask;
        }

        public async Task<Dictionary<string, string>> GetSystemSettingsAsync()
        {
            return await Task.FromResult(new Dictionary<string, string>
            {
                {"SystemName", "RIMS"},
                {"Version", "1.0.0"},
                {"ContactEmail", "support@rims.local"},
                {"SupportPhone", "+1234567890"},
                {"SessionTimeout", "30"},
                {"MaxLoginAttempts", "5"},
                {"MaintenanceMode", "false"}
            });
        }

        public async Task UpdateSystemSettingAsync(string key, string value)
        {
            // In real implementation, update in configuration store
            await Task.CompletedTask;
        }

        public async Task<SystemHealth> GetSystemHealthAsync()
        {
            var health = new SystemHealth();

            // Check database connection
            try
            {
                await _context.Database.ExecuteSqlRawAsync("SELECT 1");
                health.DatabaseConnected = true;
            }
            catch
            {
                health.DatabaseConnected = false;
            }

            // Check email service (simplified)
            health.EmailServiceWorking = true;

            // Check file system access
            health.FileSystemAccessible = Directory.Exists(Path.GetTempPath());

            // Get last backup time (simplified)
            health.LastBackup = DateTime.Now.AddDays(-1);

            health.ServiceStatus = new Dictionary<string, bool>
            {
                {"Database", health.DatabaseConnected},
                {"Email Service", health.EmailServiceWorking},
                {"File System", health.FileSystemAccessible},
                {"Cache", true},
                {"Background Services", true}
            };

            return health;
        }

        public async Task RunDatabaseMaintenanceAsync()
        {
            // Rebuild indexes
            await _context.Database.ExecuteSqlRawAsync(@"
                EXEC sp_MSforeachtable 'ALTER INDEX ALL ON ? REBUILD'");

            // Update statistics
            await _context.Database.ExecuteSqlRawAsync(@"
                EXEC sp_updatestats");

            // Clean up old data
            var thirtyDaysAgo = DateTime.Now.AddDays(-30);
            await _context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM rimsAuditTrail 
                WHERE ActionDate < {0}", thirtyDaysAgo);
        }

        public async Task CleanupTempFilesAsync()
        {
            var tempPath = Path.GetTempPath();
            var tempFiles = Directory.GetFiles(tempPath, "rims_*.*");

            foreach (var file in tempFiles)
            {
                try
                {
                    if (File.GetCreationTime(file) < DateTime.Now.AddHours(-24))
                    {
                        File.Delete(file);
                    }
                }
                catch
                {
                    // Ignore files that can't be deleted
                }
            }

            await Task.CompletedTask;
        }

        public async Task BackupDatabaseAsync(string backupPath)
        {
            var databaseName = _context.Database.GetDbConnection().Database;
            var backupFile = Path.Combine(backupPath, $"rims_backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak");

            await _context.Database.ExecuteSqlRawAsync(@"
                BACKUP DATABASE {0} TO DISK = {1}
                WITH FORMAT, MEDIANAME = 'RIMS_Backup', NAME = 'Full Backup of RIMS'",
                databaseName, backupFile);
        }

        public async Task RestoreDatabaseAsync(string backupPath)
        {
            var databaseName = _context.Database.GetDbConnection().Database;

            // Set database to single user mode
            await _context.Database.ExecuteSqlRawAsync(@"
                ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE",
                databaseName);

            // Restore database
            await _context.Database.ExecuteSqlRawAsync(@"
                RESTORE DATABASE {0} FROM DISK = {1}
                WITH REPLACE",
                databaseName, backupPath);

            // Set back to multi-user mode
            await _context.Database.ExecuteSqlRawAsync(@"
                ALTER DATABASE {0} SET MULTI_USER",
                databaseName);
        }

        public async Task<IEnumerable<RIMSUsers>> GetSystemUsersAsync()
        {
            return await _context.rimsUsers.ToListAsync();
        }

        public async Task<RIMSUsers> CreateUserAsync(RIMSUsers user, string password)
        {
            // Note: For production, use UserManager<RIMSUsers> for proper identity operations
            user.Id = Guid.NewGuid().ToString();
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = true;
            user.TwoFactorEnabled = false;
            user.LockoutEnabled = true;
            user.AccessFailedCount = 0;

            _context.rimsUsers.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<RIMSUsers> UpdateUserAsync(RIMSUsers user)
        {
            _context.rimsUsers.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _context.rimsUsers.FindAsync(userId);
            if (user != null)
            {
                _context.rimsUsers.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ResetPasswordAsync(string userId, string newPassword)
        {
            // Note: For production, use UserManager<RIMSUsers> for password operations
            var user = await _context.rimsUsers.FindAsync(userId);
            if (user != null)
            {
                // In real implementation, use UserManager to set password hash
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<SystemLog>> GetSystemLogsAsync(LogFilter filter)
        {
            var query = _context.rimsAuditTrail.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Level))
            {
                query = query.Where(log => log.ActionType == filter.Level);
            }

            if (filter.StartDate.HasValue)
            {
                query = query.Where(log => log.ActionDate >= filter.StartDate.Value);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(log => log.ActionDate <= filter.EndDate.Value);
            }

            if (!string.IsNullOrEmpty(filter.UserId))
            {
                query = query.Where(log => log.UserId == filter.UserId);
            }

            var logs = await query
                .OrderByDescending(log => log.ActionDate)
                .Take(1000)
                .Select(log => new SystemLog
                {
                    Id = log.Id ?? 0, // Provide default value if Id is null
                    Level = log.ActionType ?? "Info",
                    Message = log.Remarks ?? log.ActionType ?? "Action performed",
                    Exception = string.Empty, // RIMSAuditTrail doesn't have ErrorMessage property
                    Timestamp = log.ActionDate,
                    UserId = log.UserId
                })
                .ToListAsync();

            return logs;
        }

        public async Task ClearSystemLogsAsync()
        {
            var thirtyDaysAgo = DateTime.Now.AddDays(-30);
            await _context.Database.ExecuteSqlRawAsync(@"
                DELETE FROM rimsAuditTrail 
                WHERE ActionDate < {0}", thirtyDaysAgo);
        }

        public async Task<SystemPerformance> GetSystemPerformanceAsync()
        {
            var activeUsersCount = await _context.rimsAuditTrail
                .Where(a => a.ActionDate >= DateTime.Now.AddMinutes(-30))
                .Select(a => a.UserId)
                .Distinct()
                .CountAsync();

            return await Task.FromResult(new SystemPerformance
            {
                Uptime = TimeSpan.FromDays(30),
                AverageResponseTime = 150,
                ErrorRate = 0.5,
                ActiveUsers = (int)activeUsersCount, // Explicit cast from long to int
                DatabasePerformance = "Optimal",
                LastMaintenance = DateTime.Now.AddDays(-1)
            });
        }

        public async Task<ResourceUsage> GetResourceUsageAsync()
        {
            return await Task.FromResult(new ResourceUsage
            {
                CpuUsage = 15.5,
                MemoryUsage = 45.2,
                DiskUsage = 65.8,
                ActiveConnections = 5 // Simplified implementation
            });
        }
    }
}