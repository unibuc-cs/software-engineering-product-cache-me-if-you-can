using Developer_Toolbox.Data;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;

namespace Developer_Toolbox.Repositories
{
    public class LockedExerciseRepository : ILockedExerciseRepository
    {
        private readonly ApplicationDbContext _context;

        public LockedExerciseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<LockedExercise> GetAllLockedExercises()
        {
            return _context.LockedExercises.ToList();
        }

        public LockedExercise GetLockedExerciseById(int? id)
        {
            return _context.LockedExercises.FirstOrDefault(e => e.Id == id);
        }
    }
}