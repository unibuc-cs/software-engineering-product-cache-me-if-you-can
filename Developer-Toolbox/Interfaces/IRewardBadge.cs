using Developer_Toolbox.Models;

namespace Developer_Toolbox.Interfaces
{
    public interface IRewardBadge
    {
        void RewardPostQuestionBadge(Badge badge, string userId);

        void RewardPostAnswerBadge(Badge badge, string userId);

        void RewardBeUpvotedBadge(Badge badge, string userId);

        void RewardSolveExerciseBadge(Badge badge, string userId);

        void RewardAddExerciseBadge(Badge badge, string userId);

        void RewardAddChallengeBadge(Badge badge, string userId);

        void RewardCompleteChallengeBadge(Badge badge, string userId);
    }
}
