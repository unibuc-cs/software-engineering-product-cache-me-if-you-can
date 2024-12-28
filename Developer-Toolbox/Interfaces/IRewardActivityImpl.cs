using Developer_Toolbox.Data;
using Developer_Toolbox.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Channels;

namespace Developer_Toolbox.Interfaces
{
    public class IRewardActivityImpl : IRewardActivity
    {
        private readonly ApplicationDbContext db;

        public IRewardActivityImpl(ApplicationDbContext context)
        {
            db = context;
        }

        public void RewardActivity(int activityId, string userId, bool cancel = false)
        {
            var reward = db.Activities.First(act => act.Id == activityId)?.ReputationPoints;
            if (reward == null) { return; }

            var user = db.ApplicationUsers.Where(user => user.Id == userId).First();
            if (user == null) { return; }

            if (cancel == true)
            {
                user.ReputationPoints -= reward;
            }
            else
            {
                user.ReputationPoints += reward;
            }

            db.SaveChanges();

        }

        public void RewardCompleteChallenge(WeeklyChallenge challenge, string userId)
        {
            // Lista de ExerciseIds asociate acestui WeeklyChallenge
            var exerciseIds = from ce in db.WeeklyChallengeExercises
                              where ce.WeeklyChallengeId == challenge.Id
                              select ce.ExerciseId;

            // Numărul total de exerciții asociate provocării
            var nrTotal = exerciseIds.Count();

            // Filtrăm soluțiile pentru a le lua doar pe cele care au data CreatedAt între StartDate și EndDate
            var nrSolutii = db.Solutions
                .Where(s => s.UserId == userId
                    && s.Score == 100
                    && s.ExerciseId.HasValue
                    && exerciseIds.Contains(s.ExerciseId.Value)
                    && s.CreatedAt >= challenge.StartDate.Date
                    && s.CreatedAt <= challenge.EndDate.AddDays(1).Date)
                .Select(s => s.ExerciseId)
                .Distinct()
                .Count();

            if (nrSolutii >= 1)
            {
                var reward = db.Activities.First(act => act.Id == (int)ActivitiesEnum.COMPLETE_CHALLENGE)?.ReputationPoints;
                if (reward == null) { return; }

                var user = db.ApplicationUsers.Where(user => user.Id == userId).First();
                if (user == null) { return; }

                user.ReputationPoints += reward;
            }

            db.SaveChanges();
        }
    }
}
