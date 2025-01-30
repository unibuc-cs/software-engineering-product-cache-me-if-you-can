using Developer_Toolbox.Data;
using Developer_Toolbox.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Developer_Toolbox.Controllers
{
    public class ApplicationUsersController : Controller
    {

       private readonly ApplicationDbContext db;
       private readonly UserManager<ApplicationUser> _userManager;
       private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;



        public ApplicationUsersController(ApplicationDbContext context,
           UserManager<ApplicationUser> userManager,
           RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        // Conditiile de afisare a butoanelor de editare si stergere
        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;

            ViewBag.EsteAdmin = User.IsInRole("Admin");

            ViewBag.EsteModerator = User.IsInRole("Moderator");

            ViewBag.UserCurent = _userManager.GetUserId(User);
        }


        // lista tututor userilor si search bar

        public IActionResult Index(string search)
        {
            SetAccessRights();
            var users = db.ApplicationUsers.Where(u => !string.IsNullOrEmpty(u.UserName)); // Convertim DbSet în interogare

            if (!string.IsNullOrEmpty(search))
            {
                // Cautăm după nume sau prenume
                users = users.Where(a => a.UserName.Contains(search));
            }

            ViewBag.Users = users.ToList(); // Obținem lista completă de utilizatori după aplicarea filtrului
            ViewBag.SearchString = search;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.MessageType = TempData["messageType"];
            }
            return View();
        }

        //LEADERBOARD
        public IActionResult Leaderboard(string search)
        {
            SetAccessRights();

            // Obținem mai întâi lista completă ordonată
            var allUsers = db.ApplicationUsers
                .Where(u => !string.IsNullOrEmpty(u.UserName))
                .OrderByDescending(user => user.ReputationPoints)
                .ToList();

            ViewBag.AllUsersList = allUsers;

            // Modificăm căutarea pentru a fi case-insensitive
            if (!string.IsNullOrEmpty(search))
            {
                ViewBag.Users = allUsers
                    .Where(a =>                       
                        a.UserName.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            else
            {
                ViewBag.Users = allUsers;
            }

            ViewBag.SearchString = search;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.MessageType = TempData["messageType"];
            }
            return View();
        }



        //afisarea unui singur profil in functie de id-ul sau
        public IActionResult Show(string? id)
        {
            SetAccessRights();
            
                if (!User.Identity.IsAuthenticated)
                {
                    // Dacă utilizatorul nu este autentificat, direcționează-l către pagina de înregistrare
                    return Redirect("/Identity/Account/Login");
                }

                //ApplicationUser currentUser = _userManager.GetUserAsync(User).Result;

            ApplicationUser user = db.ApplicationUsers
                          .Where(u => u.Id == id)
                          .FirstOrDefault();

               ViewBag.User = user;

            var questions = db.Questions.Include("User").Where(q => q.UserId == id);
            var answers = db.Answers.Include("User").Where(a => a.UserId == id);

            var answerCount = db.Answers.Count(a => a.UserId == id);

            var questionCount = db.Questions.Count(a => a.UserId == id);


            ViewBag.Questions = questions;
            ViewBag.Answers = answers;
            ViewBag.AnswerCount = answerCount;
            ViewBag.QuestionCount = questionCount;
            ViewBag.Scor = user.ReputationPoints;


            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.MessageType = TempData["messageType"];
            }

            return View();
        }

        // formularul in care se vor completa datele unei profil nouu
        public IActionResult New()
        {
            ApplicationUser user = new ApplicationUser();

            return View(user);
        }


        // Se adauga profilul in baza de date

        [HttpPost]
        public IActionResult New(ApplicationUser user)
        {

            user.Id = _userManager.GetUserId(User);
            user.ReputationPoints = 0;

            db.ApplicationUsers.Add(user);

            db.SaveChanges();
            TempData["message"] = "Profile created successfully!";
            TempData["messageType"] = "alert-success";

            return View(user);

        }

        // Se editeaza un profil existent in baza de date 
        // HttpGet implicit
        // Se afiseaza formularul impreuna cu datele aferente profilului din baza de date

        [Authorize(Roles ="User,Moderator,Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ApplicationUser user = db.ApplicationUsers
                                        .Where(u => u.Id == _userManager.GetUserId(User))
                                        .First();
            if (user.Id == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                user.Id = _userManager.GetUserId(User);
                ViewBag.User = user;
                return View(user);
            }
            else
            {

                TempData["message"] = "You are not allowed to edit a profile that is not yours!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
            
        }

        [Authorize(Roles = "User,Moderator,Admin")]
        // Se adauga profilul modificat in baza de date
        [HttpPost]
        public IActionResult Edit(string id, ApplicationUser requestProfile)
        {
            ApplicationUser user = db.Users.Find(id);

            user.UserName = requestProfile.UserName;
            user.Description = requestProfile.Description;
            user.Birthday= requestProfile.Birthday;

            if (user.Id == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                TempData["message"] = "You edited your profile successfully!";
                TempData["messageType"] = "alert-success";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {

                TempData["message"] = "You are not allowed to edit a profile that is not yours!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
            
        }

        [Authorize(Roles = "User,Moderator,Admin")]
        public async Task<ActionResult> Delete(string id)
        {

            if (_userManager.GetUserId(User) ==id || User.IsInRole("Admin"))
            {
                var notifications = db.Notifications.Where(n => n.UserId == id).ToList();
                foreach (Notification notification in notifications)
                {
                    db.Notifications.Remove(notification);
                }
                db.SaveChanges();

                ApplicationUser user = db.ApplicationUsers
                                       .Where(user => user.Id == id).First();

                if (user == null)
                {
                    // Handle the case where user is not found
                    TempData["message"] = "Couldn't find user.";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index");
                }

                db.ApplicationUsers.Remove(user);
                TempData["message"] = "Your profile has been deleted.";
                TempData["messageType"] = "alert-success";
                db.SaveChanges();

                await _signInManager.SignOutAsync();

                // Redirect to the Index page
                return RedirectToAction("Index");
            }
            else
            {

                TempData["message"] = "You are not allowed to delete a profile that isn't yours!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }

        }

        [Authorize(Roles = "User,Moderator,Admin")]
        public IActionResult MyBadges()
        {

            var badges = db.UserBadges.Include("Badge")
                .Where(ub => ub.UserId == _userManager.GetUserId(User))
                .OrderByDescending(ub => ub.ReceivedAt).Select(ub => new
                {
                    Title = ub.Badge.Title,
                    Description = ub.Badge.Description,
                    AuthorId = ub.Badge.AuthorId,
                    Image = ub.Badge.Image,
                    Id = ub.BadgeId,
                    ReceivedAt = ub.ReceivedAt
                }).ToList();

            ViewBag.Badges = badges;
            return View();

        }


    }

}