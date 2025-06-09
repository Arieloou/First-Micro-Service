namespace NotificationsService.Application.Interfaces
{
    public interface INotificationProvider  : IServiceProvider
    {
        string providerType { get; }
        Task<bool> SendAsync(string recipient, string subject, string body);
    }
}
