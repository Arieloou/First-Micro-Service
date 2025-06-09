using Microsoft.Extensions.Logging;
using NotificationsService.Application.Interfaces;

namespace NotificationsService.Infraestructure.NotificationsProvider
{
    public class EmailProvider : INotificationProvider
    {
        private readonly ILogger<EmailProvider> _logger;
        private readonly string? ApiKey;
        public string providerType => "email";

        public EmailProvider(ILogger<EmailProvider> logger,IConfiguration configuration) 
        {
            _logger = logger;
            ApiKey = configuration["NotificationsProvider:EmailApiKey"] ?? "API_NOT_CONFIGURED";

            if (ApiKey == null)
            {
                _logger.LogWarning("La API Key para EmailProvider no está configurada en secretos/appsettings.");
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
