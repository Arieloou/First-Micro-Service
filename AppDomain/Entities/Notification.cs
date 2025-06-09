using NotificationsService.AppDomain.Enums;

namespace NotificationsService.AppDomain.Entities
{
    public class Notification
    {
        public Guid Id { get; private set; }
        public NotificationType Type { get; private set; }
        public string Recipient { get; private set; }
        public string? Subject { get; private set; }
        public string Body { get; private set; }
        public NotificationStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ProcessedAt { get; private set; }
        public string? FailureReason { get; private set; }

        // Constructor privado para que solo se pueda crear a través de la fábrica estática.
        private Notification(Guid id, NotificationType type, string recipient, string? subject, string body)
        {
            // Validación de reglas de negocio (invariantes)
            if (string.IsNullOrWhiteSpace(recipient))
                throw new ArgumentException("El destinatario no puede estar vacío.", nameof(recipient));
            if (string.IsNullOrWhiteSpace(body))
                throw new ArgumentException("El cuerpo no puede estar vacío.", nameof(body));
            if (type == NotificationType.Email && string.IsNullOrWhiteSpace(subject))
                throw new ArgumentException("El asunto es obligatorio para notificaciones de tipo email.", nameof(subject));

            Id = id;
            Type = type;
            Recipient = recipient;
            Subject = subject;
            Body = body;
            Status = NotificationStatus.Pending; // Siempre empieza como pendiente
            CreatedAt = DateTime.UtcNow;
        }

        // Método Fábrica. Es la forma en la que se crea una nueva notificación.
        public static Notification Create(NotificationType type, string recipient, string? subject, string body)
        {
            return new Notification(Guid.NewGuid(), type, recipient, subject, body);
        }

        // Lógica de Dominio. Son métodos que cambian el estado de la entidad.
        public void MarkAsSent()
        {
            if (Status != NotificationStatus.Pending)
                throw new InvalidOperationException("Solo se puede marcar como enviada una notificación pendiente.");

            Status = NotificationStatus.Sent;
            ProcessedAt = DateTime.UtcNow;
            FailureReason = null;
        }

        public void MarkAsFailed(string reason)
        {
            if (Status != NotificationStatus.Pending)
                throw new InvalidOperationException("Solo se puede marcar como fallida una notificación pendiente.");

            Status = NotificationStatus.Failed;
            ProcessedAt = DateTime.UtcNow;
            FailureReason = reason;
        }
    }
}
