using NotificationsService.AppDomain.Entities;
using NotificationsService.AppDomain.Interfaces;

namespace NotificationsService.Infraestructure.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDBContext _context;
        public NotificationRepository(ApplicationDBContext context) 
        { 
            _context = context;
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notification.AddAsync(notification);
            //await _context.SaveChangesAsync();
        }

        public async Task<Notification> GetById(Guid id)
        {
            return (await _context.Notification.FindAsync(id))!;
        }
    }
}
