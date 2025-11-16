using Microsoft.EntityFrameworkCore;
using RIMS.Data;
using RIMS.Models;
using RIMS.Models.Entities;
using System.Security.Claims;

namespace RIMS.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendApplicationStatusNotificationAsync(int applicationId, string recipientEmail)
        {
            var application = await _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Include(da => da.Document)
                .FirstOrDefaultAsync(da => da.Id == applicationId);

            if (application != null && application.Resident != null && application.Document != null)
            {
                // Email functionality removed - log instead
                Console.WriteLine($"Application status notification for app {applicationId} to: {recipientEmail}");
                Console.WriteLine($"Status: {application.Status}, Document: {application.Document.DocumentName}");
            }
        }

        public async Task SendCertificateReadyNotificationAsync(int applicationId)
        {
            var application = await _context.rimsDocumentApplication
                .Include(da => da.Resident)
                .Include(da => da.Document)
                .FirstOrDefaultAsync(da => da.Id == applicationId);

            if (application != null && application.Resident != null && application.Document != null)
            {
                // Email functionality removed - log instead
                Console.WriteLine($"Certificate ready notification for application: {applicationId}");
                Console.WriteLine($"Document: {application.Document.DocumentName}, Resident: {application.Resident.FirstName} {application.Resident.LastName}");

                // Also create in-app notification
                await CreateInAppNotificationAsync(new Notification
                {
                    Title = "Certificate Ready",
                    Message = $"Your {application.Document.DocumentName} is ready for pickup",
                    Type = "Certificate",
                    RecipientId = application.FK_ResidentId.ToString(),
                    Priority = "High"
                });
            }
        }

        public async Task SendIDCardReadyNotificationAsync(int residentId)
        {
            var resident = await _context.rimsResidents.FindAsync(residentId);
            if (resident != null)
            {
                // Email functionality removed - log instead
                Console.WriteLine($"ID Card ready notification for resident: {residentId}");
                Console.WriteLine($"Resident: {resident.FirstName} {resident.LastName}");

                await CreateInAppNotificationAsync(new Notification
                {
                    Title = "ID Card Ready",
                    Message = "Your ID Card is ready for pickup",
                    Type = "ID Card",
                    RecipientId = residentId.ToString(),
                    Priority = "High"
                });
            }
        }

        public async Task SendSystemAlertAsync(string subject, string message, string recipientType)
        {
            // For system alerts, you might want to notify all admins or specific user groups
            var users = await _context.rimsUsers
                .Where(u => u.EmailConfirmed)
                .ToListAsync();

            foreach (var user in users)
            {
                // Email functionality removed - log instead
                Console.WriteLine($"System alert to user {user.Id}: {subject} - {message}");

                await CreateInAppNotificationAsync(new Notification
                {
                    Title = subject,
                    Message = message,
                    Type = "System Alert",
                    RecipientId = user.Id,
                    Priority = "High"
                });
            }
        }

        public async Task CreateInAppNotificationAsync(Notification notification)
        {
            notification.CreatedDate = DateTime.Now;
            notification.IsRead = false;

            // In real implementation, save to database
            // _context.Notifications.Add(notification);
            // await _context.SaveChangesAsync();

            await Task.CompletedTask;
        }

        public Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId)
        {
            // In real implementation, retrieve from database
            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = 1,
                    Title = "Welcome to RIMS",
                    Message = "Welcome to the Resident Information Management System",
                    Type = "System",
                    RecipientId = userId,
                    IsRead = false,
                    CreatedDate = DateTime.Now.AddDays(-1)
                }
            };

            return Task.FromResult<IEnumerable<Notification>>(notifications);
        }

        public Task MarkNotificationAsReadAsync(int notificationId)
        {
            // In real implementation, update in database
            return Task.CompletedTask;
        }

        public Task MarkAllNotificationsAsReadAsync(string userId)
        {
            // In real implementation, update all user notifications
            return Task.CompletedTask;
        }

        public Task DeleteNotificationAsync(int notificationId)
        {
            // In real implementation, delete from database
            return Task.CompletedTask;
        }

        public Task SendSMSNotificationAsync(string phoneNumber, string message)
        {
            // Integrate with SMS gateway like Twilio, Nexmo, etc.
            // This is a placeholder implementation
            Console.WriteLine($"SMS to {phoneNumber}: {message}");
            return Task.CompletedTask;
        }

        public async Task SendBulkNotificationAsync(BulkNotificationRequest request)
        {
            foreach (var recipientId in request.RecipientIds)
            {
                await CreateInAppNotificationAsync(new Notification
                {
                    Title = request.Title,
                    Message = request.Message,
                    Type = request.Type,
                    RecipientId = recipientId,
                    Priority = "Normal"
                });

                if (request.Channel == "email" || request.Channel == "both")
                {
                    // Send email notification
                    var user = await _context.rimsUsers.FindAsync(recipientId);
                    if (user != null && !string.IsNullOrEmpty(user.Email))
                    {
                        Console.WriteLine($"Bulk email to {user.Email}: {request.Title} - {request.Message}");
                    }
                }

                if (request.Channel == "sms" || request.Channel == "both")
                {
                    // Send SMS notification
                    var resident = await _context.rimsResidents.FindAsync(int.Parse(recipientId));
                    if (resident != null && !string.IsNullOrEmpty(resident.ContactNumber))
                    {
                        await SendSMSNotificationAsync(resident.ContactNumber, request.Message);
                    }
                }
            }
        }

        public Task<NotificationStatistics> GetNotificationStatisticsAsync()
        {
            var statistics = new NotificationStatistics
            {
                TotalNotifications = 1000,
                UnreadNotifications = 150,
                NotificationsByType = new Dictionary<string, int>
                {
                    {"System", 400},
                    {"Certificate", 300},
                    {"ID Card", 200},
                    {"Application", 100}
                },
                DeliveryStatus = new Dictionary<string, int>
                {
                    {"Sent", 800},
                    {"Delivered", 750},
                    {"Failed", 50}
                }
            };

            return Task.FromResult(statistics);
        }
    }
}