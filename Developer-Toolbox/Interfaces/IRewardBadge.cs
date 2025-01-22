using Developer_Toolbox.Models;

namespace Developer_Toolbox.Interfaces
{
    public interface IRewardBadge
    {
        void RewardPostQuestionBadge(Badge badge, ApplicationUser user);

        void RewardPostAnswerBadge(Badge badge, ApplicationUser user);

        void RewardBeUpvotedBadge(Badge badge, ApplicationUser user);

        void RewardSolveExerciseBadge(Badge badge, ApplicationUser user);

        void RewardAddExerciseBadge(Badge badge, ApplicationUser user);

        void RewardAddChallengeBadge(Badge badge, ApplicationUser user);

        void RewardCompleteChallengeBadge(Badge badge, ApplicationUser user);
    }
}
