using Developer_Toolbox.Data;
using Developer_Toolbox.Interfaces;
using Developer_Toolbox.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
namespace Developer_Toolbox.Controllers
{
    public class ChallengeNotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRewardBadge _rewardBadge;
        private readonly IRewardActivity _rewardActivity;
        private readonly IEmailService _emailService;

        public ChallengeNotificationService(ApplicationDbContext context,
            IRewardBadge rewardBadge,
            IRewardActivity rewardActivity,
            IEmailService emailService)
        {
            _context = context;
            _rewardBadge = rewardBadge;
            _rewardActivity = rewardActivity;
            _emailService = emailService;   
        }

        public async Task CheckActiveChallengesAndSendNotifications()
        {
            // Obține provocările active din baza de date
            var challenges = _context.WeeklyChallenges
                .ToList();

            foreach (var challenge in challenges)
            {
                var users = _context.Users.ToList();

                foreach (var user in users)
                {
                    // Notificare la începutul provocării
                    var startNotificationExists = _context.Notifications.Any(n =>
                        (n.UserId == user.Id ||
                        n.UserId == null) &&
                        n.Link == "/WeeklyChallenges/Show/" + challenge.Id &&
                        n.Message == $"Challenge {challenge.Title} has started!");

                    if (!startNotificationExists && challenge.StartDate <= DateTime.Now && challenge.EndDate >= DateTime.Now)
                    {
                        var startNotification = new Notification
                        {
                            UserId = user.Id,
                            Message = $"Challenge {challenge.Title} has started!",
                            Link = "/WeeklyChallenges/Show/" + challenge.Id,
                            CreatedAt = DateTime.Now,
                            IsRead = false
                        };

                        _context.Notifications.Add(startNotification);
                        _emailService.SendNewChallengeEmailAsync(user.Email, user.UserName, challenge);
                    }

                    // Notificare spre sfârșitul provocării
                    var endingSoonNotificationExists = _context.Notifications.Any(n =>
                        n.UserId == user.Id &&
                        n.Link == "/WeeklyChallenges/Show/" + challenge.Id &&
                        n.Message == $"Challenge {challenge.Title} is ending soon!");

                    if (!endingSoonNotificationExists && (DateTime.Now > challenge.StartDate && DateTime.Now <= challenge.EndDate ||
                                                         DateTime.Now == challenge.StartDate && DateTime.Now.Hour < challenge.EndDate.Hour))
                    {
                        var endingSoonNotification = new Notification
                        {
                            UserId = user.Id,
                            Message = $"Challenge {challenge.Title} is ending soon!",
                            Link = "/WeeklyChallenges/Show/" + challenge.Id,
                            CreatedAt = DateTime.Now,
                            IsRead = false
                        };

                        _context.Notifications.Add(endingSoonNotification);
                        _emailService.SendAlmostEndedChallengeEmailAsync(user.Email, user.UserName, challenge);
                    }

                    // Notificare la sfârșitul provocării
                    var endNotificationExists = _context.Notifications.Any(n =>
                        n.UserId == user.Id &&
                        n.Link == "/WeeklyChallenges/Show/" + challenge.Id &&
                        n.Message == $"Challenge {challenge.Title} has ended!");

                    if (!endNotificationExists && DateTime.Now > challenge.EndDate.AddDays(1))
                    {
                        var endNotification = new Notification
                        {
                            UserId = user.Id,
                            Message = $"Challenge {challenge.Title} has ended!",
                            Link = "/WeeklyChallenges/Show/" + challenge.Id,
                            CreatedAt = DateTime.Now,
                            IsRead = false
                        };

                        await _emailService.SendEndedChallengeEmailAsync(user.Email, user.UserName, challenge);
                        RewardAfterChallengeEnded(challenge, user);

                        _context.Notifications.Add(endNotification);
                    }

                    _context.SaveChanges();
                }

                
            }

            
        }

        private void RewardAfterChallengeEnded(WeeklyChallenge challenge, ApplicationUser user)
        {
            // gasim toate badge urile care sunt asociate acestui challenge
            var badgesQuery = from bc in _context.BadgeChallenges
                         join b in _context.Badges on bc.BadgeId equals b.Id
                         where bc.WeeklyChallengeId == challenge.Id
                         select b;
            var badges = badgesQuery.ToList();

            _rewardActivity.RewardCompleteChallenge(challenge, user.Id);

            foreach (var badge in badges)
            {
                // if user has already the badge, skip
                var usersBadges = _context.UserBadges.Any(ub => ub.BadgeId == badge.Id && ub.UserId == user.Id);
                if (usersBadges) continue;

                _rewardBadge.RewardCompleteChallengeBadge(badge, user);
            }
        }

    }

}
