using Developer_Toolbox.Models;

namespace Developer_Toolbox.Repositories
{
    public interface ILockedSolutionRepository
    {
        IEnumerable<LockedSolution> GetAllLockedSolutions();
        LockedSolution GetLockedSolutionById(int? id);
    }
}