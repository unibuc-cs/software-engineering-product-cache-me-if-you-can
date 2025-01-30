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
    public class SolutionsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext db;
        private readonly ISolutionRepository _solutionRepository;
        public SolutionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ISolutionRepository solutionRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            db = context;
            _solutionRepository = solutionRepository;
        }

        [Authorize(Roles = "User,Moderator,Admin")]
        public async Task<IActionResult> Index()
        {
            // Get the current user's ID
            var userId = _userManager.GetUserId(User);

            // Retrieve solutions for the current user
            var solutions = db.Solutions.Include(s => s.Exercise)
                                                    .Include(s => s.User)
                                                    .Where(s => s.UserId == userId)
                                                    .OrderBy(s => s.Score)
                                                    .ToList();

            ViewBag.Solutions = solutions;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.MessageType = TempData["messageType"];
            }

            return View();
        }

        [Authorize(Roles ="User,Moderator,Admin")]
        public IActionResult Show(int id) 
        { 
            Solution solution = db.Solutions.Include("Exercise")
                                        .Where(sol => sol.Id == id)
                                        .First();
            if(solution != null && solution.UserId == _userManager.GetUserId(User) || User.IsInRole("Moderator") || User.IsInRole("Admin"))
            {
                return View(solution);
            }
            else
            {
                TempData["message"] = "You are not allowed to view a solution that you didn't submitted!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
            
        }


        [Authorize(Roles ="User,Moderator,Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Solution solution = db.Solutions.Find(id);

            if (solution != null && solution.UserId == _userManager.GetUserId(User) || User.IsInRole("Moderator") || User.IsInRole("Admin"))
            {
                db.Solutions.Remove(solution);
                db.SaveChanges();

                TempData["message"] = "Solution successfully deleted";
                TempData["messageType"] = "alert-success";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "You are not allowed to delete a solution that you didn't submitted!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
                
        }

        public IActionResult GetAllSolutions()
        {
            var solutions = _solutionRepository.GetAllSolutions();
            return View(solutions);
        }
        public IActionResult GetSolutionById(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solution = _solutionRepository.GetSolutionById(id);
            if (solution == null)
            {
                return NotFound();
            }

            return View(solution);
        }

    }
}

