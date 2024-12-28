using Developer_Toolbox.Models;
using Microsoft.AspNetCore.Identity;

namespace Developer_Toolbox.Interfaces
{
    public interface IRewardActivity
    {
        void RewardActivity(int activityId, string userId, bool cancel = false);

        void RewardCompleteChallenge(WeeklyChallenge challenge, string userId);
    }
}
