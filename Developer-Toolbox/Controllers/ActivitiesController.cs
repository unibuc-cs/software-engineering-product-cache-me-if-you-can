using Developer_Toolbox.Data;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Developer_Toolbox.Controllers
{
    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IActivityRepository _activityRepository;

        public ActivitiesController(ApplicationDbContext db, 
            UserManager<ApplicationUser> userManager,
            IActivityRepository activityRepository)
        {
            this.db = db;
            _userManager = userManager;
            _activityRepository = activityRepository;
        }

        private void SetAccessRights()
        {
            ViewBag.EsteAdmin = User.IsInRole("Admin");
        }

        public IActionResult Index()
        {
            SetAccessRights();

            // transmitem mesajele primite in view
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.MessageType = TempData["messageType"];
            }

            // preluam activitatile din baza de date 
            var activities = from activity in db.Activities
                             select activity;

            // transmitem activitatile in view
            ViewBag.Activities = activities.ToList<Activity>();

            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit()
        {
            // transmitem mesajele primite in view
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.MessageType = TempData["messageType"];
            }

            // preluam activitatile din baza de date 
            var activities = from activity in db.Activities
                             select activity;

            // transmitem activitatile in view
            ViewBag.Activities = activities.ToList<Activity>();

            return View(activities.ToList<Activity>());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(List<Activity> reputationPointsPerActivity)
        {
            // preluam activitatile
            var activities = from activity in db.Activities select activity;
     

            if (ModelState.IsValid)
            {
                if (User.IsInRole("Admin"))
                {

                    // modificam informatiile
                    foreach (Activity activity in activities)
                    {
                        var modifiedActivity = reputationPointsPerActivity.Find(act => act.Id == activity.Id);
                        if (modifiedActivity != null)
                        {
                            activity.ReputationPoints = modifiedActivity.ReputationPoints;
                        }
                    }

                    //commit
                    db.SaveChanges();

                    TempData["message"] = "The rewards have been edited";
                    TempData["messageType"] = "alert-success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "Only the admin can modify rewards!";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index");
                }

            }
            else
            {
                return View(activities.ToList<Activity>());
            }
        }

        // Noua metodă GetAllActivities
        public IActionResult GetAllActivities()
        {
            var activities = _activityRepository.GetAllActivities();
            return View(activities);
        }

        // Noua metodă GetActivityById
        public IActionResult GetActivityById(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = _activityRepository.GetActivityById(id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }


    }
}
