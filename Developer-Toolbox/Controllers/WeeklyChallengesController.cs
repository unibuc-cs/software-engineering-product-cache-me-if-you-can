using Developer_Toolbox.Data;
using Developer_Toolbox.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Developer_Toolbox.Controllers
{
    public class WeeklyChallengesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;

        public WeeklyChallengesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            db = context;
            _userManager = userManager;
        }

        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;
            ViewBag.EsteAdmin = User.IsInRole("Admin");
            ViewBag.UserCurent = _userManager.GetUserId(User);
            // verificam daca are profilul complet
            bool completeProfile = false;

            if (_userManager.GetUserId(User) != null)
            {
                // userConectat = true;
                if (db.ApplicationUsers.Find(_userManager.GetUserId(User)).FirstName != null)
                    completeProfile = true;
            }

            ViewBag.CompleteProfile = completeProfile;
        }

        public IActionResult Index()
        {
            SetAccessRights();

            // Preluăm datele din baza de date
            var weeklyChallenges = db.WeeklyChallenges
                                      .OrderByDescending(wc => wc.StartDate);

            // Afisare paginată
            int perPage = 3;
            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);
            if (currentPage == 0) currentPage = 1;

            var offset = (currentPage - 1) * perPage;

            var paginatedChallenges = weeklyChallenges.Skip(offset).Take(perPage).ToList();

            ViewBag.WeeklyChallenges = paginatedChallenges;
            ViewBag.LastPage = Math.Ceiling((double)weeklyChallenges.Count() / perPage);
            ViewBag.PaginationBaseUrl = "/WeeklyChallenges/Index?page";

            return View();
        }

        // Formular pentru un nou WeeklyChallenge
        [Authorize(Roles = "Admin")]
        public IActionResult New()
        {
            WeeklyChallenge weeklyChallenge = new WeeklyChallenge
            {
                StartDate = DateTime.Now
            };

            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login");
            }

            var exercises = db.Exercises
                .Select(e => new { e.Id, e.Title })
                .ToList();

            ViewBag.Exercises = exercises;
            return View(weeklyChallenge);
        }

        // POST: WeeklyChallenges/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public IActionResult New(WeeklyChallenge weeklyChallenge, [FromForm] List<int> ExerciseIds)
        {
            if (!ModelState.IsValid)
            {
                // Dacă validarea modelului nu reușește, trimitem din nou datele către view.
                var exercises = db.Exercises.Select(e => new { e.Id, e.Title }).ToList();
                ViewBag.Exercises = exercises;
                return View(weeklyChallenge);
            }

            try
            {
                // Inițializăm lista pentru relația Many-to-Many
                weeklyChallenge.WeeklyChallengeExercises = new List<WeeklyChallengeExercise>();

                foreach (var exerciseId in ExerciseIds)
                {
                    weeklyChallenge.WeeklyChallengeExercises.Add(new WeeklyChallengeExercise
                    {
                        ExerciseId = exerciseId
                    });
                }

                // Salvăm noul WeeklyChallenge
                db.WeeklyChallenges.Add(weeklyChallenge);
                db.SaveChanges();

                TempData["message"] = "The weekly challenge has been successfully added.";
                TempData["messageType"] = "alert-success";

                return RedirectToAction("Index"); // Redirect către pagina Index (sau alta acțiune)
            }
            catch (Exception ex)
            {
                TempData["message"] = "An error occurred while saving the weekly challenge.";
                TempData["messageType"] = "alert-danger";
                return View(weeklyChallenge);
            }
        }


        public IActionResult Show(int id)
        {
            SetAccessRights();

            // Preluăm WeeklyChallenge din baza de date, inclusiv exercițiile asociate
            var weeklyChallenge = db.WeeklyChallenges
                .Include(wc => wc.WeeklyChallengeExercises)
                .ThenInclude(wce => wce.Exercise)
                .FirstOrDefault(wc => wc.Id == id);

            if (weeklyChallenge == null)
            {
                TempData["message"] = "The requested Weekly Challenge does not exist.";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }

            // Lista de ExerciseIds asociate acestui WeeklyChallenge
            var exerciseIds = weeklyChallenge.WeeklyChallengeExercises
                .Select(wce => wce.ExerciseId)
                .ToList();

            // Obținem utilizatorul curent
            var userId = _userManager.GetUserId(User);

            // Numărul total de exerciții asociate provocării
            var nrTotal = exerciseIds.Count;

            // Filtrăm soluțiile pentru a le lua doar pe cele care au data CreatedAt între StartDate și EndDate
            var nrSolutii = db.Solutions
                .Where(s => s.UserId == userId
                    && s.Score == 100
                    && s.ExerciseId.HasValue
                    && exerciseIds.Contains(s.ExerciseId.Value)
                    && s.CreatedAt >= weeklyChallenge.StartDate.Date // Folosim doar data (fără ora)
                    && s.CreatedAt <= weeklyChallenge.EndDate.AddDays(1).Date) // Folosim doar data (fără ora)
                .Select(s => s.ExerciseId) // Obținem doar ExerciseId-urile pentru soluții corecte
                .Distinct() // Ne asigurăm că fiecare exercițiu este numărat o singură dată
                .Count();

            // Stocăm raportul în ViewBag
            ViewBag.SolutionsSummary = $"{nrSolutii}/{nrTotal}";

            // Transmitem WeeklyChallenge către view
            ViewBag.WeeklyChallenge = weeklyChallenge;
            return View();
        }




        // GET: Edit Weekly Challenge
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            SetAccessRights();

            // Preluăm WeeklyChallenge din baza de date, inclusiv exercițiile asociate
            var weeklyChallenge = db.WeeklyChallenges
                .Include(wc => wc.WeeklyChallengeExercises)
                .ThenInclude(wce => wce.Exercise)
                .FirstOrDefault(wc => wc.Id == id);

            // Verificăm dacă WeeklyChallenge există
            if (weeklyChallenge == null)
            {
                TempData["message"] = "The requested Weekly Challenge does not exist.";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }

            // Adăugăm lista de exerciții în ViewBag pentru a o folosi în formular
            ViewBag.Exercises = db.Exercises.ToList();
            return View(weeklyChallenge); // Returnăm WeeklyChallenge în view pentru editare
        }

        // POST: Update Weekly Challenge
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, WeeklyChallenge updatedChallenge, List<int> ExerciseIds)
        {
            // Preluăm WeeklyChallenge din baza de date, inclusiv exercițiile asociate
            var weeklyChallenge = db.WeeklyChallenges
                .Include(wc => wc.WeeklyChallengeExercises)
                .FirstOrDefault(wc => wc.Id == id);

            // Verificăm dacă WeeklyChallenge există
            if (weeklyChallenge == null)
            {
                TempData["message"] = "The requested Weekly Challenge does not exist.";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }

            // Verificăm validitatea modelului
            if (ModelState.IsValid)
            {
                // Actualizăm datele provocării
                weeklyChallenge.Title = updatedChallenge.Title;
                weeklyChallenge.Description = updatedChallenge.Description;
                weeklyChallenge.Difficulty = updatedChallenge.Difficulty;
                weeklyChallenge.RewardPoints = updatedChallenge.RewardPoints;
                weeklyChallenge.StartDate = updatedChallenge.StartDate;  // Actualizare StartDate
                weeklyChallenge.EndDate = updatedChallenge.EndDate;      // Actualizare EndDate

                // Actualizăm lista de exerciții asociate
                weeklyChallenge.WeeklyChallengeExercises.Clear(); // Ștergem vechile exerciții asociate
                foreach (var exerciseId in ExerciseIds)
                {
                    weeklyChallenge.WeeklyChallengeExercises.Add(new WeeklyChallengeExercise
                    {
                        WeeklyChallengeId = id,
                        ExerciseId = exerciseId
                    });
                }

                // Salvăm modificările în baza de date
                db.SaveChanges();
                TempData["message"] = "The Weekly Challenge was successfully updated.";
                TempData["messageType"] = "alert-success";

                // Redirecționăm către pagina Show pentru a vizualiza provocarea actualizată
                return RedirectToAction("Show", new { id = weeklyChallenge.Id });
            }

            // Dacă validarea nu a reușit, adăugăm lista de exerciții și returnăm formularul de editare
            ViewBag.Exercises = db.Exercises.ToList();
            return View(updatedChallenge); // Returnăm modelul actualizat cu erorile de validare
        }




        // GET: Confirm Delete Weekly Challenge
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            SetAccessRights();

            var weeklyChallenge = db.WeeklyChallenges
                .Include(wc => wc.WeeklyChallengeExercises)
                .ThenInclude(wce => wce.Exercise)
                .FirstOrDefault(wc => wc.Id == id);

            if (weeklyChallenge == null)
            {
                TempData["message"] = "The requested Weekly Challenge does not exist.";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }

            // Redirecționează direct la Index, fără a returna un View
            return RedirectToAction("Index");
        }

        // POST: Delete Weekly Challenge
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var weeklyChallenge = db.WeeklyChallenges
                .Include(wc => wc.WeeklyChallengeExercises)
                .FirstOrDefault(wc => wc.Id == id);

            if (weeklyChallenge == null)
            {
                TempData["message"] = "The requested Weekly Challenge does not exist.";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }

            // Ștergere relații din tabelul intermediar
            db.WeeklyChallengeExercises.RemoveRange(weeklyChallenge.WeeklyChallengeExercises);

            // Ștergere challenge
            db.WeeklyChallenges.Remove(weeklyChallenge);
            db.SaveChanges();

            TempData["message"] = "The Weekly Challenge was successfully deleted.";
            TempData["messageType"] = "alert-success";
            return RedirectToAction("Index");
        }



    }
}
