using Developer_Toolbox.Data;
using Developer_Toolbox.Models;
using System.Collections.Generic;
using System.Linq;

namespace Developer_Toolbox.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Notification> GetAllNotifications()
        {
            return _context.Notifications.ToList();
        }

        public Notification GetNotificationById(int? id)
        {
            return _context.Notifications.FirstOrDefault(n => n.Id == id);
        }
    }
}