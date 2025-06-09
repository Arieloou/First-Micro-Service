using Microsoft.EntityFrameworkCore;
using NotificationsService.AppDomain.Entities;
using NotificationsService.Application.DTOs;

namespace NotificationsService.Infraestructure.Persistence
{
    public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : DbContext(options)
    {
        //public DbSet<NotificationRequest> NotificationRequest { get; set; }
        public DbSet<NotificationLog> NotificationLogs { get; set; }
        public DbSet<Notification> Notification { get; set; }
    }
}
