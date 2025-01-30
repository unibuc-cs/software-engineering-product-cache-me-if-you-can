using Developer_Toolbox.Data;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Developer_Toolbox.Controllers
{
    public class LearningPathsController : Controller
    {
        private readonly ApplicationDbContext db;
        private IWebHostEnvironment _env;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        
        public LearningPathsController(ApplicationDbContext context,
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = environment;
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
            SetAccessRights();
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.MessageType = TempData["messageType"];
            }
            var paths = db.LearningPaths;
            ViewBag.Paths = paths;
            return View(paths);
        }

        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult New()
        {
            //transmitem mesajele primite in view
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.MessageType = TempData["messageType"];
            }

            // noul path
            LearningPath path = new LearningPath();

            return View(path);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        public async Task<IActionResult> New(LearningPath path)
        {
            SetAccessRights();

            path.UserId = _userManager.GetUserId(User);

            
            if (ModelState.IsValid)
            {

                
                db.LearningPaths.Add(path);
               
                db.SaveChanges();

                TempData["message"] = "The learning path has been added";
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
                return View(path);
            }
        }

        public IActionResult Show(int id)
        {
            SetAccessRights();


            //transmitem mesajele primite in view
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.MessageType = TempData["messageType"];
            }

            SetAccessRights();

            // preluam path-ul cerut
            LearningPath path = db.LearningPaths.Include("LockedExercises")
                                            .Include("User")
                                            .Where(path => path.Id == id)
                                            .First();

            ViewBag.Ex = db.LockedExercises.Include("User").Where(ex => ex.LearningPathId == id);

            ViewBag.LastEx = db.LockedExercises.Include("User").Where(ex => ex.LearningPathId == id
                              && db.LockedSolutions.Any(s => s.LockedExerciseId == ex.Id && s.Score == 100 && s.UserId== _userManager.GetUserId(User)))
                              .OrderByDescending(ex => ex.Id)
                              .FirstOrDefault();



            return View(path);

        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            // preluam path ul care trebuie sters
            LearningPath path = db.LearningPaths.Include("LockedExercises")
                                             .Where(c => c.Id == id)
                                             .First();

            if (path.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
              

               
                db.LearningPaths.Remove(path);

                // commit
                db.SaveChanges();

                TempData["message"] = "The path has been deleted";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "You're unable to delete a path you didn't add!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
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

            LearningPath path = db.LearningPaths.Find(id);

            //restrictionam permisiunile
            if (path.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View(path);
            }
            else
            {
                TempData["message"] = "You're unable to modify a path you didn't add!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, LearningPath requestPath)
        {
            
            LearningPath path = db.LearningPaths.Find(id);


            if (ModelState.IsValid)
            {
                if (path.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {


                    // modificam informatiile
                    path.Name = requestPath.Name;
                    path.Description = requestPath.Description;

                 

                    //commit
                    db.SaveChanges();

                    TempData["message"] = "The path has been edited";
                    TempData["messageType"] = "alert-success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "You're unable to modify a path you didn't add!";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index");
                }

            }
            else
            {
                return View(requestPath);
            }
        }

    }
}
