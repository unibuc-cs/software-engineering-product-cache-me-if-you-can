using Developer_Toolbox.Data;
using Developer_Toolbox.Models;
using System.Collections.Generic;
using System.Linq;

namespace Developer_Toolbox.Repositories
{
    public class WeeklyChallengeRepository : IWeeklyChallengeRepository
    {
        private readonly ApplicationDbContext _context;

        public WeeklyChallengeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<WeeklyChallenge> GetAllWeeklyChallenges()
        {
            return _context.WeeklyChallenges.ToList();
        }

        public WeeklyChallenge GetWeeklyChallengeById(int? id)
        {
            return _context.WeeklyChallenges.FirstOrDefault(wc => wc.Id == id);
        }
    }
}
