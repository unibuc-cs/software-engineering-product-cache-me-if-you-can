using Developer_Toolbox.Data;
using Developer_Toolbox.Models;
using System.Collections.Generic;
using System.Linq;

namespace Developer_Toolbox.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly ApplicationDbContext _context;

        public ActivityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Activity> GetAllActivities()
        {
            return _context.Activities.ToList();
        }

        public Activity GetActivityById(int? id)
        {
            return _context.Activities.FirstOrDefault(a => a.Id == id);
        }
    }
}