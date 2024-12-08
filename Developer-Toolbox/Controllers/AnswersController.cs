using Developer_Toolbox.Data;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
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

        public AnswersController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IAnswerRepository answerRepository)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _answerRepository = answerRepository;
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
        [HttpPost]
        public IActionResult Delete(int id)
        {
            SetAccessRights();
            Answer answ = db.Answers.Find(id);
            db.Answers.Remove(answ);
            db.SaveChanges();

            return Redirect("/Questions/Show/" + answ.QuestionId);
        }



        public IActionResult Edit(int id)
        {
            SetAccessRights();
            Answer answ = db.Answers.Find(id);
            ViewBag.Answer = answ;
            return View();
        }

        [HttpPost]
        public IActionResult Edit(int id, Answer requestAnswer)
        {
            Answer answ = db.Answers.Find(id);
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

                int noAnswersPosted;

                if (badge.BadgeTags?.Count != 0)
                {
                    // check if the user posted more than TargetNoOfTimes answers to questions having tags in BadgeTags

                    var answersPosted = db.Answers.Include("Question").Include("Question.QuestionTags").Where(a => a.UserId == _userManager.GetUserId(User)).ToList();
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
                    noAnswersPosted = db.Answers.Count(q => q.UserId == _userManager.GetUserId(User));
                }

                if (noAnswersPosted >= badge.TargetNoOfTimes)
                {
                    // assign badge
                    db.UserBadges.Add(new UserBadge
                    {
                        UserId = _userManager.GetUserId(User),
                        BadgeId = badge.Id,
                        ReceivedAt = DateTime.Now
                    });

                }
            }

            db.SaveChanges();

        }

    }
}
