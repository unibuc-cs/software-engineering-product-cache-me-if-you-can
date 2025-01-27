using Developer_Toolbox.Data;
using Developer_Toolbox.Interfaces;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Developer_Toolbox.Controllers
{
    public class AnswersController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAnswerRepository _answerRepository;
        private readonly IRewardBadge _IRewardBadge;

        public AnswersController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IAnswerRepository answerRepository,
            IRewardBadge iRewardBadge)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _answerRepository = answerRepository;
            _IRewardBadge = iRewardBadge;
        }

        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;


            ViewBag.EsteAdmin = User.IsInRole("Admin");

            ViewBag.UserCurent = _userManager.GetUserId(User);
        }


        // Adaugarea unui răspuns asociat unei întrebări in baza de date
        [HttpPost]
        public IActionResult New(Answer answ)
        {
            answ.Date = DateTime.Now;
            if (!User.Identity.IsAuthenticated)
            {
                // Dacă utilizatorul nu este autentificat, direcționează-l către pagina de înregistrare
                return Redirect("/Identity/Account/Login");
            }
            ApplicationUser user = db.ApplicationUsers.Where(user => user.Id == _userManager.GetUserId(User)).FirstOrDefault();
            
            if (user == null)
                return Redirect("/ApplicationUsers/New");
            answ.UserId = _userManager.GetUserId(User);
            
            try
            {
                db.Answers.Add(answ);
                db.SaveChanges();

                RewardActivity((int)ActivitiesEnum.POST_ANSWER);
                RewardBadge();

                return Redirect("/Questions/Show/" + answ.QuestionId);
            }

            catch (Exception)
            {
                return Redirect("/Questions/Show/" + answ.QuestionId);
            }

        }

        // Stergerea unui răspuns asociat unei întrebări din baza de date
        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            SetAccessRights();
            Answer answ = db.Answers.Find(id);

            if (answ?.UserId == _userManager.GetUserId(User) || User.IsInRole("Moderator") || User.IsInRole("Admin"))
            {
                db.Answers.Remove(answ);
                db.SaveChanges();

            }
            else
            {
                TempData["message"] = "You are not allowed to delete an answer that you didn't post!";
            }

            return Redirect("/Questions/Show/" + answ.QuestionId);
        }


        [Authorize(Roles = "Admin,Moderator,User")]
        public IActionResult Edit(int id)
        {
            SetAccessRights();
            Answer answ = db.Answers.Find(id);

            if (answ?.UserId == _userManager.GetUserId(User) || User.IsInRole("Moderator") || User.IsInRole("Admin"))
            {
                ViewBag.Answer = answ;
                return View();
            }
            else
            {
                TempData["message"] = "You are not allowed to delete an answer that you didn't post!";
                 return Redirect("/Questions/Show/" + answ.QuestionId);
            }              
        }

        [Authorize(Roles = "Admin,Moderator,User")]
        [HttpPost]
        public IActionResult Edit(int id, Answer requestAnswer)
        {
            Answer answ = db.Answers.Find(id);

            if (answ?.UserId == _userManager.GetUserId(User) || User.IsInRole("Moderator") || User.IsInRole("Admin"))
            {
                try
                {

                    answ.Content = requestAnswer.Content;

                    db.SaveChanges();

                    return Redirect("/Questions/Show/" + answ.QuestionId);
                }
                catch (Exception e)
                {
                    return Redirect("/Questions/Show/" + answ.QuestionId);
                }
            }
            else
            {
                TempData["message"] = "You are not allowed to edit an answer that you didn't post!";
                return Redirect("/Questions/Show/" + answ.QuestionId);
            }       
        }

        // Noua metodă GetAllAnswers
        public IActionResult GetAllAnswers()
        {
            var answers = _answerRepository.GetAllAnswers();
            return View(answers);
        }

        // Noua metodă GetAnswerById
        public IActionResult GetAnswerById(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = _answerRepository.GetAnswerById(id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        [NonAction]
        private void RewardActivity(int activityId)
        {
            var reward = db.Activities.First(act => act.Id == activityId)?.ReputationPoints;
            if (reward == null) { return; }

            var user = db.ApplicationUsers.Where(user => user.Id == _userManager.GetUserId(User)).First();
            if (user == null) { return; }

            user.ReputationPoints += reward;

            db.SaveChanges();

        }

        [NonAction]
        private void RewardBadge()
        {
            var badges = db.Badges.Include("BadgeTags").Where(b => b.TargetActivity.Id == (int)ActivitiesEnum.POST_ANSWER).ToList();
            if (badges == null) { return; }

            foreach (var badge in badges)
            {
                // if user has already the badge, skip
                var usersBadges = db.UserBadges.Any(ub => ub.BadgeId == badge.Id && ub.UserId == _userManager.GetUserId(User));
                if (usersBadges) continue;

                _IRewardBadge.RewardPostAnswerBadge(badge, _userManager.GetUserId(User));
                
            }

        }

    }
}
