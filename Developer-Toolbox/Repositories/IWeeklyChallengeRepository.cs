using Developer_Toolbox.Models;
using System.Collections.Generic;

namespace Developer_Toolbox.Repositories
{
    public interface IWeeklyChallengeRepository
    {
        IEnumerable<WeeklyChallenge> GetAllWeeklyChallenges();
        WeeklyChallenge GetWeeklyChallengeById(int? id);
    }
}
