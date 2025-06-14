using Microsoft.AspNetCore.Http.HttpResults;
using NotificationsService.Application.Interfaces;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Runtime;

namespace NotificationsService.Infraestructure.NotificationsProvider
{
    public class EmailProvider : INotificationProvider
    {
        private readonly ILogger<EmailProvider> _logger;
        private readonly SmtpClient smtpClient;

        private readonly string addressFrom;
        private readonly string displayName;

        public string providerType => "email";

        public EmailProvider(ILogger<EmailProvider> logger,IConfiguration configuration)
        {
            _logger = logger;

            addressFrom = configuration["EmailSettings:From"] ?? "";
            displayName = configuration["EmailSettings:DisplayName"] ?? "";
            
            // The password value is only used in the Email Provider Constructor
            string password = configuration["EmailSettings:Password"] ?? "";

            if (string.IsNullOrEmpty(addressFrom) || string.IsNullOrEmpty(addressFrom))
            {
                _logger.LogWarning("The EmailProvider From or DisplayName is not configured in .env.");
                throw new Exception("The EmailProvider From or DisplayName is not configured in .env.");
            }

            smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(addressFrom,password)
            };
        }

        public async Task<bool> SendAsync(string recipient, string subject, string body)
        {
            try
            {
                var message = new MailMessage();
                message.From = new MailAddress(addressFrom, displayName);
                message.To.Add(recipient);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                
                await smtpClient.SendMailAsync(message);

                _logger.LogInformation($"Mensaje enviado a {recipient} por medio de: {providerType}, encabezado:{subject}, mensaje:{body}");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar enviar EMAIL a {Recipient}.", recipient);

                return false;
            }
        }
    }
}
