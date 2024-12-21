using Developer_Toolbox.Data;
using Developer_Toolbox.Interfaces;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Developer_Toolbox.Controllers
{
    public class ReactionsController : Controller
    {

        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IReactionRepository _reactionRepository;
        private readonly IRewardBadge _IRewardBadge;
        private readonly IEmailService _IEmailService;

        public ReactionsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IReactionRepository reactionRepository,
            IRewardBadge iRewardBadge,
            IEmailService emailService)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _reactionRepository = reactionRepository;
            _IRewardBadge = iRewardBadge;
            _IEmailService = emailService;
        }

        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;


            ViewBag.EsteAdmin = User.IsInRole("Admin");

            ViewBag.UserCurent = _userManager.GetUserId(User);
        }

        //Metoda care realizeaza actiunea pentru like-ul unei intrebari
        //Nu se poate realiza like si dislike simultan la aceeasi intrebare de catre acelasi utilizator
        public IActionResult LikeQuestion(int questionId)
        {
            var userCurent = _userManager.GetUserId(User);
            var question = db.Questions.Find(questionId);
            if (question != null)
            {
                if (db.Reactions.Any(r => r.UserId == userCurent && r.QuestionId == questionId))
                {
                    Reaction reaction = db.Reactions.Where(r => r.UserId == userCurent && r.QuestionId == question.Id).FirstOrDefault();
                    if(reaction.Liked == false)
                    {
                        if (reaction.Disliked == true)
                        {
                            reaction.Disliked = false;
                            question.DislikesNr--;
                            
                        }
                        reaction.Liked = true;
                        // Incrementam numărului de like-uri
                        question.LikesNr++;
                        RewardActivity((int)ActivitiesEnum.BE_UPVOTED);
                    }
                    
                   
                }
                else
                {
                    var reaction = new Reaction { UserId = userCurent, QuestionId = questionId };
                    reaction.Disliked = false;
                    reaction.Liked = true;
                    // Incrementam numărului de like-uri
                    question.LikesNr++;
                    db.Reactions.Add(reaction);
                    RewardActivity((int)ActivitiesEnum.BE_UPVOTED);
                }

                
                db.SaveChanges();

                RewardBadge(question.UserId);

                return Redirect("/Questions/Show/" + questionId);// Redirecționam la pagina intrebarii
            }
            // Tratam cazul în care întrebarea nu există sau alte erori
            return NotFound();
        }

        //Metoda care realizeaza actiunea pentru dislike-ul unei intrebari
        //Nu se poate realiza like si dislike simultan la aceeasi intrebare de catre acelasi utilizator

        public IActionResult DislikeQuestion(int questionId)
        {
            var userCurent = _userManager.GetUserId(User);
            var question = db.Questions.Find(questionId);
            if (question != null)
            {
                if (db.Reactions.Any(b => b.UserId == userCurent && b.QuestionId == questionId))
                {
                    Reaction reaction = db.Reactions.Where(r => r.UserId == userCurent && r.QuestionId == question.Id).FirstOrDefault();
                    if(reaction.Disliked == false)
                    {
                        if (reaction.Liked == true)
                        {
                            reaction.Liked = false;
                            question.LikesNr--;
                            RewardActivity((int)ActivitiesEnum.BE_UPVOTED, true);
                        }
                        reaction.Disliked = true;
                        // Incrementam numărului de dislike-uri
                        question.DislikesNr++;
                }
                }
                else
                {
                    var reaction = new Reaction { UserId = userCurent, QuestionId = questionId };
                    reaction.Liked = false;
                    reaction.Disliked = true;
                    // Incrementam numărului de dislike-uri
                    question.DislikesNr++;
                    db.Reactions.Add(reaction);
                }


                db.SaveChanges();
                return Redirect("/Questions/Show/" + questionId);// Redirecționam la pagina intrebarii
            }
            // Tratam cazul în care întrebarea nu există sau alte erori
            return NotFound();
        }

        //Metoda care realizeaza actiunea din apasarea dubla a unui buton de like, mai exact Undo-Like, cand utilizatorul
        //doreste sa isi retraga likeul de la o intrebare

        public IActionResult UndoLikeQuestion(int questionId)
        {
            var userCurent = _userManager.GetUserId(User);
            var question = db.Questions.Find(questionId);
            if (question != null)
            {
                if (db.Reactions.Any(b => b.UserId == userCurent && b.QuestionId == questionId))
                {
                    Reaction reaction = db.Reactions.Where(r => r.UserId == userCurent && r.QuestionId == question.Id).FirstOrDefault();
                    reaction.Liked = false;
                    // Decrementam numărului de like-uri
                    question.LikesNr--;
                    RewardActivity((int)ActivitiesEnum.BE_UPVOTED, true);
                }


                db.SaveChanges();
                return Redirect("/Questions/Show/" + questionId);// Redirecționam la pagina intrebarii
            }
            // Tratam cazul în care întrebarea nu există sau alte erori
            return NotFound();
        }

        //Metoda care realizeaza actiunea din apasarea dubla a unui buton de dislike, mai exact Undo-Dislike, cand utilizatorul
        //doreste sa isi retraga dislikeul de la o intrebare
        public IActionResult UndoDislikeQuestion(int questionId)
        {
            var userCurent = _userManager.GetUserId(User);
            var question = db.Questions.Find(questionId);
            if (question != null)
            {
                if (db.Reactions.Any(b => b.UserId == userCurent && b.QuestionId == questionId))
                {
                    Reaction reaction = db.Reactions.Where(r => r.UserId == userCurent && r.QuestionId == question.Id).FirstOrDefault();
                    reaction.Disliked = false;
                    // Decrementam numărului de dislike-uri
                    question.DislikesNr--;
                }


                db.SaveChanges();
                return Redirect("/Questions/Show/" + questionId);// Redirecționam la pagina intrebarii
            }
            // Tratam cazul în care întrebarea nu există sau alte erori
            return NotFound();
        }

        // Adăugăm metode pentru a utiliza repository-ul

        public IActionResult GetAllReactions()
        {
            var reactions = _reactionRepository.GetAllReactions();
            return View(reactions);
        }

        public IActionResult GetReactionById(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reaction = _reactionRepository.GetReactionById(id.Value);
            if (reaction == null)
            {
                return NotFound();
            }

            return View(reaction);
        }


        [NonAction]
        private void RewardActivity(int activityId, bool cancel = false)
        {
            var reward = db.Activities.First(act => act.Id == activityId)?.ReputationPoints;
            if (reward == null) { return; }

            var user = db.ApplicationUsers.Where(user => user.Id == _userManager.GetUserId(User)).First();
            if (user == null) { return; }

            if (cancel == true)
            {
                user.ReputationPoints -= reward;
            } else
            {
                user.ReputationPoints += reward;
            }
            db.SaveChanges();

        }

        [NonAction]
        private async void RewardBadge(string questionAuthorId)
        {
            var badges = db.Badges.Include("BadgeTags").Where(b => b.TargetActivity.Id == (int)ActivitiesEnum.BE_UPVOTED).ToList();
            if (badges == null) { return; }

            foreach (var badge in badges)
            {
                // if user has already the badge, skip
                var usersBadges = db.UserBadges.Any(ub => ub.BadgeId == badge.Id && ub.UserId == questionAuthorId);
                if (usersBadges) continue;

                _IRewardBadge.RewardBeUpvotedBadge(badge, questionAuthorId);

                ApplicationUser user = await _userManager.GetUserAsync(User);
                string userEmail = await _userManager.GetEmailAsync(user);
                string userName = await _userManager.GetUserNameAsync(user);
                await _IEmailService.SendBadgeAwardedEmailAsync(userEmail, userName, badge);
            }

        }

    }
}
