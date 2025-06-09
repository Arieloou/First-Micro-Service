using Microsoft.Extensions.Logging;
using NotificationsService.Application.Interfaces;

namespace NotificationsService.Infraestructure.NotificationsProvider
{
    public class SMSProvider : INotificationProvider
    {
        private readonly ILogger<SMSProvider> _logger;
        private readonly string? ApiKey;

        public string providerType => "SMS";

        public SMSProvider (ILogger<SMSProvider> logger, IConfiguration configuration)
        {
            _logger = logger;
            ApiKey = configuration["NotificationsProvider:SMSApiKey"] ?? "API_NOT_CONFIGURED";

            if (ApiKey == null)
            {
                _logger.LogWarning("La API Key para SMSProvider no está configurada en secretos/appsettings.");
            }
        }

        public Task<bool> SendAsync(string recipient, string subject, string body)
        {
            try
            {
                // Aquí iría la lógica real de envío...

                _logger.LogInformation($"Mensaje enviado a {recipient} por medio de: {providerType}, encabezado:{subject}, mensaje:{body}");

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar enviar EMAIL a {Recipient}.", recipient);

                return Task.FromResult(false);
            }
        }
    }
}
