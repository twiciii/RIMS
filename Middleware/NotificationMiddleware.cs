using Microsoft.EntityFrameworkCore;
using RIMS.Data;
using RIMS.Models.Entities;

namespace RIMS.Middleware
{
    public class NotificationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public NotificationMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Process notifications before the request
            await ProcessPendingNotifications();

            await _next(context);

            // Could add post-request notification processing here
        }

        private async Task ProcessPendingNotifications()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Check for pending document applications that need attention
            var pendingApplications = await dbContext.rimsDocumentApplication
                .Where(da => da.Status == "Pending")
                .CountAsync();

            if (pendingApplications > 0)
            {
                // In a real implementation, you might store this in a notification system
                // or push to connected clients via SignalR
                Console.WriteLine($"Notification: {pendingApplications} pending applications need review");
            }

            // Check for applications expiring soon (within 3 days)
            var expiringSoon = await dbContext.rimsDocumentApplication
                .Where(da => da.ExpirationDate.HasValue &&
                            da.ExpirationDate.Value.Date <= DateTime.Now.AddDays(3).Date &&
                            da.Status != "Completed")
                .CountAsync();

            if (expiringSoon > 0)
            {
                Console.WriteLine($"Notification: {expiringSoon} applications expiring soon");
            }

            // Check for overdue applications (pending for more than 7 days)
            var overdueApplications = await dbContext.rimsDocumentApplication
                .Where(da => da.Status == "Pending" &&
                            da.ApplicationDate.Date <= DateTime.Now.AddDays(-7).Date)
                .CountAsync();

            if (overdueApplications > 0)
            {
                Console.WriteLine($"Notification: {overdueApplications} applications are overdue");
            }

            // Add other notification checks as needed
            // - System maintenance alerts
            // - New resident registrations needing verification
        }
    }

    public static class NotificationMiddlewareExtensions
    {
        public static IApplicationBuilder UseNotificationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<NotificationMiddleware>();
        }
    }
}