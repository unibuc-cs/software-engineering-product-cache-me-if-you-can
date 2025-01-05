using Developer_Toolbox.Data;
using Developer_Toolbox.Interfaces;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Developer_Toolbox.Controllers
{
    public class BadgesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IWebHostEnvironment _env;
        private readonly IRewardBadge _IRewardBadge;
        private readonly IBadgeRepository _badgeRepository;

        public BadgesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment environment,
            IRewardBadge iRewardBadge,
            IBadgeRepository badgeRepository)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = environment;
            _IRewardBadge = iRewardBadge;
            _badgeRepository = badgeRepository;
        }

        //Conditii de afisare a butoanelor de editare si stergere
        private void SetAccessRights()
        {
            ViewBag.IsModerator = User.IsInRole("Moderator");

            ViewBag.IsAdmin = User.IsInRole("Admin");

            ViewBag.CurrentUser = _userManager.GetUserId(User);

            // verificam daca are profilul complet
            bool userProfilComplet = false;

            if (_userManager.GetUserId(User) != null)
            {
                // userConectat = true;
                if (db.ApplicationUsers.Find(_userManager.GetUserId(User)).FirstName != null)
                    userProfilComplet = true;
            }

            ViewBag.UserProfilComplet = userProfilComplet;
        }

        public IActionResult Index()
        {
            // transmitem mesajele primite in view
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.MessageType = TempData["messageType"];
            }

            SetAccessRights();

            // preluam categoriile din baza de date 
            var badges = from badge in db.Badges
                             select badge;


            // afisare paginata
            int _perPage = 10;

            int totalItems = badges.Count();

            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);

            var offset = 0;

            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }

            var paginatedBadges = badges.Skip(offset).Take(_perPage);

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);

            // transmitem badge urile in view
            ViewBag.Badges = paginatedBadges;

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
            badge.TargetNoOfTimes = 1;

            return View(initBadge(badge));
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
                if (isDuplicate(badge))
                {
                    TempData["message"] = "The badge hasn't been added because it is a duplicate!";
                    TempData["messageType"] = "alert-danger";

                    SetAccessRights();

                    return RedirectToAction("Index");
                }
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

                RewardBadge(badge);

                TempData["message"] = "The badge has been added";
                TempData["messageType"] = "alert-success";

                SetAccessRights();

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

                badge.TargetNoOfTimes = 1;
                return View(initBadge(badge));
            }
        }

        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Edit(int id)
        {
            //transmitem mesajele primite in view
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.MessageType = TempData["messageType"];
            }

            // preluam badge ul cautat
            Badge badge = db.Badges.Find(id);
            badge = initBadge(badge);

            if (!(bool)badge.TargetActivity.isPracticeRelated)
            {
                var tags = from bt in db.BadgeTags
                           join t in db.Tags on bt.TagId equals t.Id
                           where bt.BadgeId == badge.Id
                           select new SelectListItem
                           {
                               Value = t.Id.ToString(),
                               Text = t.Name,
                           };
                ViewBag.SelectedTags = tags.ToList();
            } else
            {
                ViewBag.SelectedTags = new List<SelectListItem>();
            }

            //restrictionam permisiunile
            if (badge.AuthorId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View(badge);
            }
            else
            {
                SetAccessRights();

                TempData["message"] = "You're unable to modify a badge you didn't add!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Badge requestBadge, IFormFile file)
        {
            SetAccessRights();

            // preluam badge-ul cautat
            Badge badge = db.Badges.Find(id);
            requestBadge = initBadge(requestBadge);
            requestBadge.TargetNoOfTimes = badge.TargetNoOfTimes;
            requestBadge.TargetActivityId = badge.TargetActivityId;

            // incercam sa uploadam imaginea
            var res = await SaveImage(file);

            if (res == null)
            {
                ModelState.AddModelError("Image", "Please load a jpg, jpeg, png or gif file type.");
            }

            ModelState.Remove("TargetNoOfTimes");
            ModelState.Remove("TargetActivityId");

            if (ModelState.IsValid)
            {
                if (badge.AuthorId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {
                    //stergem imaginea anterioara din folder-ul imgs
                    string path = Path.Join(_env.WebRootPath, badge.Image.Replace('/', '\\'));
                    System.IO.File.Delete(path);

                    // modificam informatiile
                    badge.Title = requestBadge.Title;
                    badge.Description = requestBadge.Description;

                    if (file != null && file.Length > 0)
                    {
                        badge.Image = res;

                    }

                    //commit
                    db.SaveChanges();

                    TempData["message"] = "The badge has been edited";
                    TempData["messageType"] = "alert-success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "You're unable to modify a badge you didn't add!";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index");
                }

            }
            else
            {
                badge = initBadge(badge);
                badge.Title = requestBadge.Title;
                badge.Description = requestBadge.Description;

                if (!(bool)badge.TargetActivity.isPracticeRelated)
                {
                    var tags = from bt in db.BadgeTags
                               join t in db.Tags on bt.TagId equals t.Id
                               where bt.BadgeId == badge.Id
                               select new SelectListItem
                               {
                                   Value = t.Id.ToString(),
                                   Text = t.Name,
                               };
                    ViewBag.SelectedTags = tags.ToList();
                }
                else
                {
                    ViewBag.SelectedTags = new List<SelectListItem>();
                }
                return View(badge);
            }
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            SetAccessRights();

            // verificam daca badge-ul a fost obtinut de useri, caz in care nu mai poate fi sters
            var alreadyUsed = db.UserBadges.Where(ub => ub.BadgeId == id).Any();

            if(!alreadyUsed)
            {
                // preluam badge-ul care trebuie sters
                Badge badge = db.Badges.Include("BadgeTags")
                                       .Include("UserBadges")
                                       .Where(b => b.Id == id)
                                       .First();

                if (badge.AuthorId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {
                    // stergem imaginea din folder-ul imgs
                    string path = Path.Join(_env.WebRootPath, badge.Image.Replace('/', '\\'));
                    System.IO.File.Delete(path);

                    // stergem badge-ul din baza de date
                    db.Badges.Remove(badge);

                    // commit
                    db.SaveChanges();

                    TempData["message"] = "The badge has been deleted";
                    TempData["messageType"] = "alert-success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "You're unable to delete a badge you didn't add!";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["message"] = "You cannot remove a badge that has already been assigned to users!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
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

        [NonAction]
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

        [NonAction]
        private Badge initBadge(Badge badge)
        {

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

            return badge;
        }

        [NonAction]
        private void RewardBadge(Badge badge)
        {
            var userIds = db.ApplicationUsers.Select(u => u.Id).ToList();

            foreach (var userId in userIds) 
            {
                switch (badge.TargetActivityId)
                {
                    case (int)ActivitiesEnum.POST_QUESTION:
                        _IRewardBadge.RewardPostQuestionBadge(badge, userId);
                        break;
                    case (int)ActivitiesEnum.POST_ANSWER:
                        _IRewardBadge.RewardPostAnswerBadge(badge, userId);
                        break;
                    case (int)ActivitiesEnum.BE_UPVOTED:
                        _IRewardBadge.RewardBeUpvotedBadge(badge, userId);
                        break;
                    case (int)ActivitiesEnum.SOLVE_EXERCISE:
                        _IRewardBadge.RewardSolveExerciseBadge(badge, userId);
                        break;
                    case (int)ActivitiesEnum.ADD_EXERCISE:
                        _IRewardBadge.RewardAddExerciseBadge(badge, userId);
                        break;
                }
            }

        }

        [NonAction]
        private bool isDuplicate(Badge badge)
        {
            var badgesForTheSameActivityAndNoOfTimes = db.Badges.Include("BadgeTags").Include("TargetCategory").Where(b => b.TargetActivityId == badge.TargetActivityId && b.TargetNoOfTimes == badge.TargetNoOfTimes).ToList();
            var activity = db.Activities.Where(a => a.Id == badge.TargetActivityId).FirstOrDefault();
            if ((bool)activity.isPracticeRelated)
            {
                if (badge.TargetCategoryId != null && badge.TargetLevel != null)
                {
                    return badgesForTheSameActivityAndNoOfTimes.Any(b => b.TargetCategoryId.Equals(badge.TargetCategoryId) && b.TargetLevel.Equals(badge.TargetLevel));
                }
                else if (badge.TargetCategoryId != null)
                {
                    return badgesForTheSameActivityAndNoOfTimes.Any(b => b.TargetCategoryId.Equals(badge.TargetCategoryId));
                }
                else if (badge.TargetLevel != null)
                {
                    return badgesForTheSameActivityAndNoOfTimes.Any(b => b.TargetLevel.Equals(badge.TargetLevel));
                }
                else
                {
                    return badgesForTheSameActivityAndNoOfTimes.Any();
                }
            } 
            else if (badge.TargetActivityId == (int)ActivitiesEnum.ADD_EXERCISE)
            {
                return badgesForTheSameActivityAndNoOfTimes.Any();
            }
            else
            {
                if (badge.BadgeTags != null && badge.BadgeTags.Any())
                {
                    return badgesForTheSameActivityAndNoOfTimes.Any(b => b.BadgeTags != null && b.BadgeTags.Equals(badge.BadgeTags));
                }
                else
                {
                    return badgesForTheSameActivityAndNoOfTimes.Any();
                }
            }

        }

        // Noua metodă GetAllBadges
        public IActionResult GetAllBadges()
        {
            var badges = _badgeRepository.GetAllBadges();
            return View(badges);
        }

        // Noua metodă GetBadgeById
        public IActionResult GetBadgeById(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var badge = _badgeRepository.GetBadgeById(id);
            if (badge == null)
            {
                return NotFound();
            }

            return View(badge);
        }

    }
}
