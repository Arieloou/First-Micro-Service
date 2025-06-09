using NotificationsService.AppDomain.Entities;
using NotificationsService.AppDomain.Enums;
using NotificationsService.AppDomain.Interfaces;
using NotificationsService.Application.DTOs;
using NotificationsService.Application.Interfaces;
using NotificationsService.Infraestructure.Persistence;

namespace NotificationsService.Application.Services
{
    public class NotificationAppService
    {
        private readonly Func<string, INotificationProvider> _factoryProvider;
        private readonly INotificationRepository _repository;
        private readonly ILogger<NotificationAppService> _logger;
        private readonly ApplicationDBContext _context;

        public NotificationAppService(Func<string, INotificationProvider> factoryProvider, INotificationRepository repository, ILogger<NotificationAppService> logger, ApplicationDBContext context)
        {
            _factoryProvider = factoryProvider;
            _repository = repository;
            _logger = logger;
            _context = context;
        }

        public async Task<bool> CreateAndSendNotificationAsync(NotificationRequest request)
        {
            if (!Enum.TryParse<NotificationType>(request.Type, ignoreCase: true, out var notificationType))
            {
                _logger.LogWarning("Tipo de notificación no válido recibido: {NotificationType}", request.Type);
                return false;
            }

            var notification = Notification.Create(
                notificationType,
                request.Recipient,
                request.Subject,
                request.Body!
            );

            await _repository.AddAsync(notification);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Notificación {notification.Id} creada y pendiente de envío.");

            try
            {
                var provider = _factoryProvider(request.Type);

                bool success = await provider.SendAsync(request.Recipient, request.Subject, request.Body!);

                if (success) 
                {
                    notification.MarkAsSent();
                    _logger.LogInformation($"La notificación {notification.Id} fue enviada correctamente a: {request.Recipient}");
                }
                else
                {
                    notification.MarkAsFailed("El proveedor externo indicó un fallo.");
                    _logger.LogWarning($"Notificación {notification.Id} marcada como fallida");
                }
            }
            catch (Exception ex)
            {
                notification.MarkAsFailed(ex.Message);
                _logger.LogError(ex, $"Excepción al procesar Notificación {notification.Id}");
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
