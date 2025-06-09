using System.ComponentModel.DataAnnotations;

namespace NotificationsService.Application.DTOs
{
    public class NotificationRequest
    {
        [Required]
        public string Type { get; set; }
        [Required]
        public string Recipient { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string? Body { get; set; }
    }
}
