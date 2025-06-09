using NotificationsService.AppDomain.Entities;

namespace NotificationsService.AppDomain.Interfaces
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);
        Task<Notification> GetById(Guid id);
    }
}
