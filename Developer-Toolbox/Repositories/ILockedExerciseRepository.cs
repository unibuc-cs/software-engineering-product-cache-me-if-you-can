using Developer_Toolbox.Models;

namespace Developer_Toolbox.Repositories
{
    public interface ILockedExerciseRepository
    {
        IEnumerable<LockedExercise> GetAllLockedExercises();
        LockedExercise GetLockedExerciseById(int? id);
    }
}