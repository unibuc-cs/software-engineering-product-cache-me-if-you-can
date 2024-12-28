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

        public IRewardBadgeImpl(ApplicationDbContext context)
        {
            db = context;
        }

        public void RewardPostQuestionBadge(Badge badge, string userId)
        {
            int noQuestionsPosted;

            if (badge.BadgeTags != null && badge.BadgeTags.Count != 0)
            {
                // check if the user posted more than TargetNoOfTimes questions having tags in BadgeTags

                var questionsPosted = db.Questions.Include("QuestionTags").Where(q => q.UserId == userId).ToList();
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
                noQuestionsPosted = db.Questions.Count(q => q.UserId == userId);
            }

            if (noQuestionsPosted >= badge.TargetNoOfTimes)
            {
                // assign badge
                db.UserBadges.Add(new UserBadge
                {
                    UserId = userId,
                    BadgeId = badge.Id,
                    ReceivedAt = DateTime.Now
                });

            }

            db.SaveChanges();
        }

        public void RewardPostAnswerBadge(Badge badge, string userId)
        {

            int noAnswersPosted;

            if (badge.BadgeTags != null &&  badge.BadgeTags.Count != 0)
            {
                // check if the user posted more than TargetNoOfTimes answers to questions having tags in BadgeTags

                var answersPosted = db.Answers.Include("Question").Include("Question.QuestionTags").Where(a => a.UserId == userId).ToList();
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
                noAnswersPosted = db.Answers.Count(q => q.UserId == userId);
            }

            if (noAnswersPosted >= badge.TargetNoOfTimes)
            {
                // assign badge
                db.UserBadges.Add(new UserBadge
                {
                    UserId = userId,
                    BadgeId = badge.Id,
                    ReceivedAt = DateTime.Now
                });

            }

            db.SaveChanges();
        }

        public void RewardBeUpvotedBadge(Badge badge, string userId)
        {
            int noUpvotes = 0;

            var questionsPosted = db.Questions.Include("QuestionTags").Where(q => q.UserId == userId).ToList();

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
                    UserId = userId,
                    BadgeId = badge.Id,
                    ReceivedAt = DateTime.Now
                });

            }

            db.SaveChanges();
        }

        public void RewardSolveExerciseBadge(Badge badge, string userId)
        {
            int noExercisesSolved = 0;

            var exercisesSolved = db.Solutions.Include("Exercise").Where(s => s.UserId == userId && s.Score == 100).Select(s => s.Exercise).Distinct().ToList();

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
                    UserId = userId,
                    BadgeId = badge.Id,
                    ReceivedAt = DateTime.Now
                });

            }

            db.SaveChanges();
        }

        public void RewardAddExerciseBadge(Badge badge, string userId)
        {
            int noExercisesPosted = db.Exercises.Count(ex => ex.UserId == userId);

            if (noExercisesPosted >= badge.TargetNoOfTimes)
            {
                // assign badge
                db.UserBadges.Add(new UserBadge
                {
                    UserId = userId,
                    BadgeId = badge.Id,
                    ReceivedAt = DateTime.Now
                });

            }

            db.SaveChanges();
        }

        public void RewardAddChallengeBadge(Badge badge, string userId)
        {
            int noChallengesPosted = db.WeeklyChallenges.Count(ex => ex.UserId == userId);

            if (noChallengesPosted >= badge.TargetNoOfTimes)
            {
                // assign badge
                db.UserBadges.Add(new UserBadge
                {
                    UserId = userId,
                    BadgeId = badge.Id,
                    ReceivedAt = DateTime.Now
                });

            }

            db.SaveChanges();
        }

        public void RewardCompleteChallengeBadge(Badge badge, string userId)
        {
            // take all exercises ids of the challenges targetted in the badge
            var exercisesToBeSolved = from bc in db.BadgeChallenges
                                      join ce in db.WeeklyChallengeExercises on bc.WeeklyChallengeId equals ce.WeeklyChallengeId
                                      where bc.BadgeId == badge.Id
                                      select ce.ExerciseId;

            // take all 100 scored usere's submissions of the exercises
            var submissions = from s in db.Solutions
                              join e in exercisesToBeSolved on s.ExerciseId equals e
                              where s.UserId == userId && s.Score == 100
                              group s by e into g
                              select g.Count();

            // if there is at least one 100 scored submission of every exercise, than the user has completed the challenge
            if (submissions.Count(g => g >= 1) == exercisesToBeSolved.Count()) {

                // assign badge
                db.UserBadges.Add(new UserBadge
                {
                    UserId = userId,
                    BadgeId = badge.Id,
                    ReceivedAt = DateTime.Now
                });
            }

            db.SaveChanges();
        }
    }
}
