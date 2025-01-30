using Developer_Toolbox.Data;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;

namespace Developer_Toolbox.Repositories
{
    public class LockedSolutionRepository : ILockedSolutionRepository
    {
        private readonly ApplicationDbContext _context;

        public LockedSolutionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<LockedSolution> GetAllLockedSolutions()
        {
            return _context.LockedSolutions.ToList();
        }

        public LockedSolution GetLockedSolutionById(int? id)
        {
            return _context.LockedSolutions.FirstOrDefault(s => s.Id == id);
        }
    }
}