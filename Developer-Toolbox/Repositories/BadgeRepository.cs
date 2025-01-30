using Developer_Toolbox.Data;
using Developer_Toolbox.Models;
using System.Collections.Generic;
using System.Linq;

namespace Developer_Toolbox.Repositories
{
    public class BadgeRepository : IBadgeRepository
    {
        private readonly ApplicationDbContext _context;

        public BadgeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Badge> GetAllBadges()
        {
            return _context.Badges.ToList();
        }

        public Badge GetBadgeById(int? id)
        {
            return _context.Badges.FirstOrDefault(b => b.Id == id);
        }
    }
}
