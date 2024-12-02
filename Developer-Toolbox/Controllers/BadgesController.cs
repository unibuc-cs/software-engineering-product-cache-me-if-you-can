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
        private IWebHostEnvironment _env;

        public BadgesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment environment)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = environment;
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
            badge.TargetActivities = ActivitiesToSelectItems(activities);

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
        public async Task<IActionResult> New(Badge badge, IFormFile file)
        {

            if (badge.TargetNoOfTimes  <= 0)
            {
                ModelState.AddModelError("TargetNoOfTimes", "The number of times the activity must be completed cannot be 0 or negative!");
            }

            if (ModelState.IsValid)
            {
                badge.AuthorId = _userManager.GetUserId(User);

                // incercam sa uploadam imaginea pentru logo
                var res = await SaveImage(file);

                if (res == null)
                {
                    ModelState.AddModelError("Image", "Please load a jpg, jpeg, png, and gif file type.");
                }
                else
                {
                    badge.Image = res;
                }

                db.Badges.Add(badge);
                db.SaveChanges();

                if(badge.SelectedTagsIds != null)
                {
                    foreach (var tagId in badge.SelectedTagsIds)
                    {
                        db.BadgeTags.Add(new BadgeTag(badge.Id, tagId));
                    }
                    db.SaveChanges();
                }

                TempData["message"] = "The Badge has been added";
                TempData["messageType"] = "alert-success";

                return RedirectToAction("Index");

            }
            else
            {
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        // pentru debug
                        var errorMessage = error.ErrorMessage;
                        var exception = error.Exception;
                    }
                }

                var activities = from act in db.Activities
                                 select act;

                badge.AllTargetActivities = activities.ToList();

                // preluam activitatile posibile pentru dropdown
                badge.TargetActivities = ActivitiesToSelectItems(activities);

                // preluam categoriile posibile pentru dropdown
                badge.TargetCategories = GetAllCategories();


                // setam dificultatile pentru dropdown
                ViewBag.LevelOfDifficulty = GetAllLevelsOfDifficulty();

                // preluam tagurile posibile pentru dropdown
                badge.TargetTagsItems = GetAllTags();

                return View(badge);
            }
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
        public IEnumerable<SelectListItem> ActivitiesToSelectItems(IQueryable<Activity> activities)
        {
            // preluam activitatile disponibile pentru dropdown
            var selectList = new List<SelectListItem>();

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

        private async Task<string?> SaveImage(IFormFile file)
        {
            if (file == null)
            {
                return null;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return null;
            }

            var uploadsFolder = Path.Combine("img", "badges");
            var webRootPath = _env.WebRootPath;

            var uploadsFolderPath = Path.Combine(webRootPath, uploadsFolder);

            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var uniqueFileName = $"{Guid.NewGuid().ToString()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var relativeFilePath = Path.Combine(uploadsFolder, uniqueFileName).Replace(Path.DirectorySeparatorChar, '/');
            return $"/{relativeFilePath}";
        }
    }
}
