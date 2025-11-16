using RIMS.Models;

namespace RIMS.Services
{
    public interface INotificationService
    {
        Task SendApplicationStatusNotificationAsync(int applicationId, string recipientEmail);
        Task SendCertificateReadyNotificationAsync(int applicationId);
        Task SendIDCardReadyNotificationAsync(int residentId);
        Task SendSystemAlertAsync(string subject, string message, string recipientType);

        Task CreateInAppNotificationAsync(Notification notification);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId);
        Task MarkNotificationAsReadAsync(int notificationId);
        Task MarkAllNotificationsAsReadAsync(string userId);
        Task DeleteNotificationAsync(int notificationId);

        Task SendSMSNotificationAsync(string phoneNumber, string message);
        Task SendBulkNotificationAsync(BulkNotificationRequest request);

        Task<NotificationStatistics> GetNotificationStatisticsAsync();
    }

    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string RecipientId { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ActionUrl { get; set; }
        public string? Priority { get; set; }
    }

    public class BulkNotificationRequest
    {
        public string[] RecipientIds { get; set; } = Array.Empty<string>();
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Channel { get; set; } = string.Empty;
    }

    public class NotificationStatistics
    {
        public int TotalNotifications { get; set; }
        public int UnreadNotifications { get; set; }
        public Dictionary<string, int> NotificationsByType { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> DeliveryStatus { get; set; } = new Dictionary<string, int>();
    }
}