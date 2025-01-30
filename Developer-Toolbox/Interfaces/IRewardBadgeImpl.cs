using Developer_Toolbox.Data;
using Developer_Toolbox.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Developer_Toolbox.Interfaces
{
    public class IRewardBadgeImpl : IRewardBadge
    {

        private readonly ApplicationDbContext db;
        private readonly IEmailService emailService;

        public IRewardBadgeImpl(ApplicationDbContext context, IEmailService emailService)
        {
            db = context;
            this.emailService = emailService;  
        }

        public async void RewardPostQuestionBadge(Badge badge, ApplicationUser user)
        {
            int noQuestionsPosted;

            if (badge.BadgeTags != null && badge.BadgeTags.Count != 0)
            {
                // check if the user posted more than TargetNoOfTimes questions having tags in BadgeTags

                var questionsPosted = db.Questions.Include("QuestionTags").Where(q => q.UserId == user.Id).ToList();
                var badgeTags = badge.BadgeTags?.Select(b => b.TagId).ToList();

                noQuestionsPosted = 0;
                foreach (var question in questionsPosted)
                {
                    if (question.QuestionTags?.Count > 0)
                    {
                        var questionTags = question.QuestionTags.Select(q => q.TagId).ToList();
                   
                        if (questionTags.Intersect(badgeTags).Count() > 0)
                        {
                            noQuestionsPosted++;
                        }
                    }

                }
            }
            else
            {
                // check only if the user posted more than TargetNoOfTimes questions
                noQuestionsPosted = db.Questions.Count(q => q.UserId == user.Id);
            }

            if (noQuestionsPosted >= badge.TargetNoOfTimes)
            {
                // assign badge
                db.UserBadges.Add(new UserBadge
                {
                    UserId = user.Id,
                    BadgeId = badge.Id,
                    ReceivedAt = DateTime.Now
                });

                db.SaveChanges();

                await this.emailService.SendBadgeAwardedEmailAsync(user.Email, user.UserName, badge);

            }

        }

        public void RewardPostAnswerBadge(Badge badge, ApplicationUser user)
        {

            int noAnswersPosted;

            if (badge.BadgeTags != null &&  badge.BadgeTags.Count != 0)
            {
                // check if the user posted more than TargetNoOfTimes answers to questions having tags in BadgeTags

                var answersPosted = db.Answers.Include("Question").Include("Question.QuestionTags").Where(a => a.UserId == user.Id).ToList();
                Console.WriteLine(answersPosted);
                noAnswersPosted = 0;
                foreach (var answer in answersPosted)
                {
                    if (answer.Question?.QuestionTags?.Count > 0)
                    {
                        var questionTags = answer.Question.QuestionTags.Select(q => q.TagId).ToList();
                        var badgeTags = badge.BadgeTags.Select(b => b.TagId).ToList();
                        if (questionTags.Intersect(badgeTags).Count() > 0)
                        {
                            noAnswersPosted++;
                        }
                    }

                }
            }
            else
            {
                // check only if the user posted more than TargetNoOfTimes questions
                noAnswersPosted = db.Answers.Count(q => q.UserId == user.Id);
            }

            if (noAnswersPosted >= badge.TargetNoOfTimes)
            {
                // assign badge
                db.UserBadges.Add(new UserBadge
                {
                    UserId = user.Id,
                    BadgeId = badge.Id,
                    ReceivedAt = DateTime.Now
                });

                db.SaveChanges();

                this.emailService.SendBadgeAwardedEmailAsync(user.Email, user.UserName, badge);

            }

        }

        public void RewardBeUpvotedBadge(Badge badge, ApplicationUser user)
        {
            int noUpvotes = 0;

            var questionsPosted = db.Questions.Include("QuestionTags").Where(q => q.UserId == user.Id).ToList();

            if (badge.BadgeTags != null && badge.BadgeTags.Count != 0)
            {
                // check if the user has been upvoted more than TargetNoOfTimes
                foreach (var question in questionsPosted)
                {
                    if (question.QuestionTags?.Count > 0)
                    {
                        var questionTags = question.QuestionTags.Select(q => q.TagId).ToList();
                        var badgeTags = badge.BadgeTags?.Select(b => b.TagId).ToList();
                        if (questionTags.Intersect(badgeTags).Count() > 0)
                        {
                            noUpvotes += (int)(question.LikesNr == null ? 0 : question.LikesNr);
                        }
                    }

                }
            }
            else
            {
                foreach (var question in questionsPosted)
                {
                    noUpvotes += (int)question.LikesNr;

                }
            }

            // check only if the user received more upvotes than TargetNoOfTimes
            if (noUpvotes >= badge.TargetNoOfTimes)
            {
                // assign badge
                db.UserBadges.Add(new UserBadge
                {
                    UserId = user.Id,
                    BadgeId = badge.Id,
                    ReceivedAt = DateTime.Now
                });

                db.SaveChanges();

                this.emailService.SendBadgeAwardedEmailAsync(user.Email, user.UserName, badge);
            }

            
        }

        public void RewardSolveExerciseBadge(Badge badge, ApplicationUser user)
        {
            int noExercisesSolved = 0;

            var exercisesSolved = db.Solutions.Include("Exercise").Where(s => s.UserId == user.Id && s.Score == 100).Select(s => s.Exercise).Distinct().ToList();

            if (badge.TargetLevel != null && badge.TargetCategory != null)
            {
                // check if the user solved more than TargetNoOfTimes exercises having both category = TargetCategory and level = TargetLevel
                noExercisesSolved = exercisesSolved.Count(ex => ex.Category.Equals(badge.TargetCategory) && ex.Difficulty.Equals(badge.TargetLevel));
            }
            else if (badge.TargetLevel != null)
            {
                noExercisesSolved = exercisesSolved.Count(ex => ex.Difficulty.Equals(badge.TargetLevel));
            }
            else if (badge.TargetCategory != null)
            {
                noExercisesSolved = exercisesSolved.Count(ex => ex.Category.Equals(badge.TargetCategory));
            }
            else
            {
                noExercisesSolved = exercisesSolved.Count();
            }

            if (noExercisesSolved >= badge.TargetNoOfTimes)
            {
                // assign badge
                db.UserBadges.Add(new UserBadge
                {
                    UserId = user.Id,
                    BadgeId = badge.Id,
                    ReceivedAt = DateTime.Now
                });

                db.SaveChanges();

                this.emailService.SendBadgeAwardedEmailAsync(user.Email, user.UserName, badge);

            }

        }

        public void RewardAddExerciseBadge(Badge badge, ApplicationUser user)
        {
            int noExercisesPosted = db.Exercises.Count(ex => ex.UserId == user.Id);

            if (noExercisesPosted >= badge.TargetNoOfTimes)
            {
                // assign badge
                db.UserBadges.Add(new UserBadge
                {
                    UserId = user.Id,
                    BadgeId = badge.Id,
                    ReceivedAt = DateTime.Now
                });

                db.SaveChanges();

                this.emailService.SendBadgeAwardedEmailAsync(user.Email, user.UserName, badge);

            }

        }

        public void RewardAddChallengeBadge(Badge badge, ApplicationUser user)
        {
            int noChallengesPosted = db.WeeklyChallenges.Count(ex => ex.UserId == user.Id);

            if (noChallengesPosted >= badge.TargetNoOfTimes)
            {
                // assign badge
                db.UserBadges.Add(new UserBadge
                {
                    UserId = user.Id,
                    BadgeId = badge.Id,
                    ReceivedAt = DateTime.Now
                });

                db.SaveChanges();

                this.emailService.SendBadgeAwardedEmailAsync(user.Email, user.UserName, badge);

            }
            
        }

        public void RewardCompleteChallengeBadge(Badge badge, ApplicationUser user)
        {
            var badgeChallenges = from bc in db.BadgeChallenges
                                  where bc.BadgeId == badge.Id
                                  select bc.WeeklyChallengeId;

            var badgeChallengesList = badgeChallenges.ToList();

            if (!(badgeChallengesList != null && badgeChallengesList.Any()))
            {
                return;
            }

            foreach (var challengeId in badgeChallengesList)
            {

                var challenge = db.WeeklyChallenges.Find(challengeId);

                // Lista de ExerciseIds asociate acestui WeeklyChallenge
                var exerciseIds = from ce in db.WeeklyChallengeExercises
                                  where ce.WeeklyChallengeId == challengeId
                                  select ce.ExerciseId;

                // Numărul total de exerciții asociate provocării
                var nrTotal = exerciseIds.Count();

                // Filtrăm soluțiile pentru a le lua doar pe cele care au data CreatedAt între StartDate și EndDate
                var nrSolutii = db.Solutions
                    .Where(s => s.UserId == user.Id
                        && s.Score == 100
                        && s.ExerciseId.HasValue
                        && exerciseIds.Contains(s.ExerciseId.Value)
                        && s.CreatedAt >= challenge.StartDate.Date
                        && s.CreatedAt <= challenge.EndDate.AddDays(1).Date)
                    .Select(s => s.ExerciseId)
                    .Distinct() 
                    .Count();

                if (nrSolutii < 1)
                {
                    return;
                } 
            }

            // assign badge
            db.UserBadges.Add(new UserBadge
            {
                UserId = user.Id,
                BadgeId = badge.Id,
                ReceivedAt = DateTime.Now
            });

            db.SaveChanges();

            this.emailService.SendBadgeAwardedEmailAsync(user.Email, user.UserName, badge);
        }

    }
}
