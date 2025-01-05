using Developer_Toolbox.Models;
using System.Collections.Generic;

namespace Developer_Toolbox.Repositories
{
    public interface INotificationRepository
    {
        IEnumerable<Notification> GetAllNotifications();
        Notification GetNotificationById(int? id);
    }
}