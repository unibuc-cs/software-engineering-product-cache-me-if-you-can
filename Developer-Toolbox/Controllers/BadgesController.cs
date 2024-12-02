using Developer_Toolbox.Data;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Developer_Toolbox.Controllers
{
    public class BadgesController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public BadgesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IAnswerRepository answerRepository)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            // transmitem mesajele primite in view
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.MessageType = TempData["messageType"];
            }


            // preluam categoriile din baza de date 
            var badges = from badge in db.Badges
                             select badge;

            // transmitem categoriile in view
            ViewBag.Badges = badges;
            return View();
        }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult New()
        {
            //transmitem mesajele primite in view
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.MessageType = TempData["messageType"];
            }

            Badge badge = new Badge();

            var activities = from act in db.Activities
                             select act;

            badge.AllTargetActivities = activities.ToList();

            // preluam activitatile posibile pentru dropdown
            badge.TargetActivities = ActivitiesToSelectItems();

            // preluam categoriile posibile pentru dropdown
            badge.TargetCategories = GetAllCategories();


            // setam dificultatile pentru dropdown
            ViewBag.LevelOfDifficulty = GetAllLevelsOfDifficulty();

            // preluam tagurile posibile pentru dropdown
            badge.TargetTagsItems = GetAllTags();

            return View(badge);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        public IActionResult New(Badge badge)
        {

/*            var sanitizer = new HtmlSanitizer();

            // verificam daca se respecta formatul test cases
            if (ex.TestCases != null && !HasValidStructure(ex.TestCases))
            {
                ModelState.AddModelError("TestCases", "The test cases have an invalid format!");
            }

            if (ModelState.IsValid)
            {
                //protejam de cross-scripting
                ex.Restrictions = sanitizer.Sanitize(ex.Restrictions);
                ex.Examples = sanitizer.Sanitize(ex.Examples);

                db.Exercises.Add(ex);

                db.SaveChanges();

                RewardActivity((int)ActivitiesEnum.ADD_EXERCISE);

                TempData["message"] = "The Exercise has been added";
                TempData["messageType"] = "alert-success";

                return Redirect("/Exercises/Index/" + ex.CategoryId);

            }
            else
            {
                // pentru dropdown
                ex.Categories = GetAllCategories();

                List<string> optionsList = new List<string> { "Easy", "Intermediate", "Difficult" };

                // convertim List<string> in List<SelectListItem>
                List<SelectListItem> selectListItems = optionsList.Select(option =>
                    new SelectListItem { Text = option, Value = option })
                    .ToList();

                ViewBag.OptionsSelectList = selectListItems;

                return View(ex);
            }*/

             return View(badge);
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // preluam categoriile disponibile pentru dropdown
            var selectList = new List<SelectListItem>();

            var categories = from cat in db.Categories
                             select cat;

            foreach (var category in categories)
            {
                selectList.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.CategoryName
                });
            }

            return selectList;
        }

        [NonAction]
        public IEnumerable<SelectListItem> ActivitiesToSelectItems()
        {
            // preluam activitatile disponibile pentru dropdown
            var selectList = new List<SelectListItem>();

            var activities = from act in db.Activities
                             select act;

            foreach (var activity in activities)
            {
                selectList.Add(new SelectListItem
                {
                    Value = activity.Id.ToString(),
                    Text = activity.Description
                });
            }

            return selectList;
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllLevelsOfDifficulty()
        {
            // setam dificultatile pentru dropdown
            List<string> optionsList = new List<string> { 
                DifficultyLevelsEnum.Easy.ToString(), 
                DifficultyLevelsEnum.Intermediate.ToString(),
                DifficultyLevelsEnum.Difficult.ToString(),
            };

            // convertim List<string> in List<SelectListItem>
            List<SelectListItem> selectListItems = optionsList.Select(option =>
                new SelectListItem { Text = option, Value = option })
                .ToList();

            return selectListItems;
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllTags()
        {
            var selectList = new List<SelectListItem>();

            var tags = from tag in db.Tags
                             select tag;

            foreach (var tag in tags)
            {
                selectList.Add(new SelectListItem
                {
                    Value = tag.Id.ToString(),
                    Text = tag.Name
                });
            }

            return selectList;
        }
    }
}
