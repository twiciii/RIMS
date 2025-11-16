using Microsoft.EntityFrameworkCore;
using RIMS.Data;
using RIMS.Models.Entities;
using RIMS.Services.Interfaces;

namespace RIMS.Services.Implementations
{
    public class AuditService : IAuditService
    {
        private readonly ApplicationDbContext _context;

        public AuditService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogActionAsync(string actionType, string description, string userId)
        {
            var auditTrail = new RIMSAuditTrail
            {
                ActionType = actionType,
                ModuleName = "System",
                UserId = userId,
                ActionDate = DateTime.Now,
                IpAddress = "127.0.0.1",
                ActionStatus = "Success",
                Remarks = description
            };

            _context.rimsAuditTrail.Add(auditTrail);
            await _context.SaveChangesAsync();
        }

        public async Task LogErrorAsync(string methodName, string errorMessage, string? userId = null)
        {
            var auditTrail = new RIMSAuditTrail
            {
                ActionType = "Error",
                ModuleName = methodName,
                UserId = userId ?? "System",
                ActionDate = DateTime.Now,
                IpAddress = "127.0.0.1",
                ActionStatus = "Failed",
                Remarks = $"Error in {methodName}: {errorMessage}"
            };

            _context.rimsAuditTrail.Add(auditTrail);
            await _context.SaveChangesAsync();
        }

        public async Task LogLoginAsync(string userId, string ipAddress, string userAgent)
        {
            var auditTrail = new RIMSAuditTrail
            {
                ActionType = "Login",
                ModuleName = "Authentication",
                UserId = userId,
                ActionDate = DateTime.Now,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                ActionStatus = "Success",
                Remarks = "User logged in successfully"
            };

            _context.rimsAuditTrail.Add(auditTrail);
            await _context.SaveChangesAsync();
        }

        public async Task LogLogoutAsync(string userId)
        {
            var logoutTrail = new RIMSAuditTrail
            {
                ActionType = "Logout",
                ModuleName = "Authentication",
                UserId = userId,
                ActionDate = DateTime.Now,
                IpAddress = "127.0.0.1",
                ActionStatus = "Success",
                Remarks = "User logged out successfully"
            };

            _context.rimsAuditTrail.Add(logoutTrail);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RIMSAuditTrail>> GetAuditLogsAsync(AuditFilter filter)
        {
            var query = _context.rimsAuditTrail.AsQueryable();

            if (filter.StartDate.HasValue)
                query = query.Where(a => a.ActionDate >= filter.StartDate.Value);
            if (filter.EndDate.HasValue)
                query = query.Where(a => a.ActionDate <= filter.EndDate.Value);
            if (!string.IsNullOrEmpty(filter.UserId))
                query = query.Where(a => a.UserId == filter.UserId);
            if (!string.IsNullOrEmpty(filter.ActionType))
                query = query.Where(a => a.ActionType == filter.ActionType);

            return await query
                .OrderByDescending(a => a.ActionDate)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<RIMSAuditTrail>> GetUserAuditLogsAsync(string userId)
        {
            return await _context.rimsAuditTrail
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.ActionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<RIMSAuditTrail>> GetAuditLogsByDateAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);

            return await _context.rimsAuditTrail
                .Where(a => a.ActionDate >= startDate && a.ActionDate < endDate)
                .OrderByDescending(a => a.ActionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<RIMSAuditTrail>> GetAuditLogsByActionAsync(string actionType)
        {
            return await _context.rimsAuditTrail
                .Where(a => a.ActionType == actionType)
                .OrderByDescending(a => a.ActionDate)
                .ToListAsync();
        }

        // New method implementation
        public async Task<IEnumerable<RIMSAuditTrail>> GetRecentAuditLogsAsync()
        {
            return await _context.rimsAuditTrail
                .OrderByDescending(a => a.ActionDate)
                .Take(50)
                .ToListAsync();
        }

        // New method implementation
        public async Task<int> CleanupOldAuditLogsAsync(int daysToKeep)
        {
            var cutoffDate = DateTime.Now.AddDays(-daysToKeep);
            var oldLogs = await _context.rimsAuditTrail
                .Where(a => a.ActionDate < cutoffDate)
                .ToListAsync();

            _context.rimsAuditTrail.RemoveRange(oldLogs);
            return await _context.SaveChangesAsync();
        }

        public async Task<AuditStatistics> GetAuditStatisticsAsync(DateTime fromDate, DateTime toDate)
        {
            var query = _context.rimsAuditTrail
                .Where(a => a.ActionDate >= fromDate && a.ActionDate <= toDate);

            var totalActions = await query.CountAsync();
            var totalUsers = await query.Select(a => a.UserId).Distinct().CountAsync();
            var totalErrors = await query.CountAsync(a => a.ActionType == "Error");
            var totalLogins = await query.CountAsync(a => a.ActionType == "Login");

            var actionsByType = await query
                .GroupBy(a => a.ActionType)
                .Select(g => new { ActionType = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.ActionType ?? "Unknown", x => x.Count);

            var actionsByUser = await query
                .GroupBy(a => a.UserId)
                .Select(g => new { UserId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.UserId ?? "Unknown", x => x.Count);

            var actionsByModule = await query
                .GroupBy(a => a.ModuleName)
                .Select(g => new { ModuleName = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.ModuleName ?? "Unknown", x => x.Count);

            return new AuditStatistics
            {
                TotalActions = totalActions,
                TotalUsers = totalUsers,
                TotalErrors = totalErrors,
                TotalLogins = totalLogins,
                ActionsByType = actionsByType,
                ActionsByUser = actionsByUser,
                ActionsByModule = actionsByModule
            };
        }

        public async Task<byte[]> ExportAuditLogsAsync(DateTime fromDate, DateTime toDate, string format = "excel")
        {
            var logs = await _context.rimsAuditTrail
                .Where(a => a.ActionDate >= fromDate && a.ActionDate <= toDate)
                .OrderByDescending(a => a.ActionDate)
                .ToListAsync();

            // For now, return empty bytes - implement actual export logic based on format
            await Task.CompletedTask;
            return Array.Empty<byte>();
        }
    }
}