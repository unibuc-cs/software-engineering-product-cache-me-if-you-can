using Developer_Toolbox.Models;
using System.Collections.Generic;

namespace Developer_Toolbox.Repositories
{
    public interface IActivityRepository
    {
        IEnumerable<Activity> GetAllActivities();
        Activity GetActivityById(int? id);
    }
}