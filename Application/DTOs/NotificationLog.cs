using System.ComponentModel.DataAnnotations;

namespace NotificationsService.Application.DTOs
{
    public class NotificationLog
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string NotificationType { get; set; } // Puede ser Email o SMS
        [Required]
        public string Recipient { get; set; }
        public string? FailureReason { get; set; } // Para darle almacenar mensaje de error en caso de que lo hubiera

        public NotificationLog(DateTime timeStamp, string status, string notificationType, string recipient, string failureReason)
        {
            TimeStamp = timeStamp;
            Status = status;
            NotificationType = notificationType;
            Recipient = recipient;
            FailureReason = failureReason;
        }
    }
}
