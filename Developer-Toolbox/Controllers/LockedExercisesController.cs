using Developer_Toolbox.Data;
using Developer_Toolbox.Models;
using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using Developer_Toolbox.Repositories;
using Developer_Toolbox.Interfaces;


namespace Developer_Toolbox.Controllers
{
    public class LockedExercisesController : Controller
    {
        private readonly ApplicationDbContext db;
        private IWebHostEnvironment _env;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        // This is for code execution:  <summary>
        private readonly HttpClient _httpClient;
        private readonly ILockedExerciseRepository _lockedExerciseRepository;

        public LockedExercisesController(ApplicationDbContext context,
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, HttpClient httpClient,
            ILockedExerciseRepository lockedExerciseRepository)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _env = environment;
            _httpClient = httpClient;   // variable for http request to send the code
            _lockedExerciseRepository = lockedExerciseRepository;
        }

        //Conditii de afisare a butoanelor de editare si stergere
        private void SetAccessRights()
        {
            ViewBag.IsModerator = User.IsInRole("Moderator");
            ViewBag.IsAdmin = User.IsInRole("Admin");
            ViewBag.CurrentUser = _userManager.GetUserId(User);

            bool userProfilComplet = false;
            if (_userManager.GetUserId(User) != null)
            {
                if (db.ApplicationUsers.Find(_userManager.GetUserId(User)).FirstName != null)
                    userProfilComplet = true;
            }

            ViewBag.UserProfilComplet = userProfilComplet;
            ViewBag.CompleteProfile = userProfilComplet; // Add this line
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

            // noul ex
            LockedExercise ex = new LockedExercise();

            // preluam categoriile posibile pentru dropdown
            ex.LearningPaths = GetAllLearningPaths();

            // setam dificultatile pentru dropdown
            List<string> optionsList = new List<string> { "Easy", "Intermediate", "Difficult" };

            // convertim List<string> in List<SelectListItem>
            List<SelectListItem> selectListItems = optionsList.Select(option =>
                new SelectListItem { Text = option, Value = option })
                .ToList();

            ViewBag.OptionsSelectList = selectListItems;


            return View(ex);
        }

        [NonAction]
        private bool HasValidStructure(string test_cases)
        {
            Regex parsing = new Regex(@"Input\n([^\n]+)\nOutput\n([^\n]*)", RegexOptions.IgnoreCase);
            var test_cases_cleaned = test_cases.Replace("\r", "").Trim('\n');
            // impartim in teste
            var split_tests = Regex.Split(test_cases_cleaned, @"([\n]*)Test case #[0-9]+:([\n]*)", RegexOptions.IgnoreCase);
            foreach (var test_case in split_tests)
            {
                // pentru fiecare test verificam sa aiba input si output
                var test_case_cleaned = test_case.Replace("\r", "").Trim('\n').Trim();
                if (test_case_cleaned.Length == 0) continue;
                Match match = parsing.Match(test_case_cleaned);
                if (!match.Success)
                {
                    return false;
                }
            }

            return true;
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        public IActionResult New(LockedExercise ex)
        {

            var sanitizer = new HtmlSanitizer();

            ex.Date = DateTime.Now;
            ex.UserId = _userManager.GetUserId(User);
            //ex.LearningPathId = 

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

                db.LockedExercises.Add(ex);

                db.SaveChanges();

                //RewardActivity((int)ActivitiesEnum.ADD_EXERCISE);

                //RewardBadgeForAddingExercise();

                TempData["message"] = "The Exercise has been added";
                TempData["messageType"] = "alert-success";

                return Redirect("/LearningPaths/Show/" + ex.LearningPathId);

            }
            else
            {
                // pentru dropdown
                ex.LearningPaths = GetAllLearningPaths();

                List<string> optionsList = new List<string> {
                    DifficultyLevelsEnum.Easy.ToString(),
                    DifficultyLevelsEnum.Intermediate.ToString(),
                    DifficultyLevelsEnum.Difficult.ToString()
                };

                // convertim List<string> in List<SelectListItem>
                List<SelectListItem> selectListItems = optionsList.Select(option =>
                    new SelectListItem { Text = option, Value = option })
                    .ToList();

                ViewBag.OptionsSelectList = selectListItems;

                return View(ex);
            }
        }

        [Authorize]
        public IActionResult Show(int id)
        {
            SetAccessRights();

            // ex curent
            var ex = db.LockedExercises
                      .Include("User")
                      .Include("LearningPath")
                      .FirstOrDefault(ex => ex.Id == id);

            if (ex == null)
            {
                return NotFound();
            }

            var lastSolvedExercise = db.LockedSolutions
                                      .Where(s => s.UserId == _userManager.GetUserId(User) &&
                                                s.Score == 100)
                                      .OrderByDescending(s => s.LockedExerciseId)
                                      .FirstOrDefault();


            bool hasAccess = false;

            if (ViewBag.IsAdmin)
            {
                hasAccess = true;
            }
            else if (lastSolvedExercise == null && id == db.LockedExercises
                                                          .Where(e => e.LearningPathId == ex.LearningPathId)
                                                          .Min(e => e.Id))
            {

                hasAccess = true;
            }
            else if (lastSolvedExercise != null && id <= lastSolvedExercise.LockedExerciseId + 1)
            {

                hasAccess = true;
            }

            if (!hasAccess)
            {
                TempData["message"] = "You need to complete the previous exercises first!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Show", "LearningPaths", new { id = ex.LearningPathId });
            }


            ViewBag.CurrentCode = "";
            var cpp = new SelectListItem { Text = "C++", Value = "cpp" };
            var python = new SelectListItem { Text = "Python", Value = "python" };
            List<SelectListItem> programmingLanguagesList = new List<SelectListItem> { python, cpp };
            ViewBag.ProgrammingLanguagesList = programmingLanguagesList;

            return View(ex);
        }

        [Authorize(Roles = "Admin,Moderator")]
        public IActionResult Edit(int id)
        {
            // preluam exercitiul din baza de date
            LockedExercise exercise = db.LockedExercises.Include("LearningPath")
                                            .Include("User")
                                            .Where(exercise => exercise.Id == id)
                                            .First();

            // pentru dropdown categorii
            exercise.LearningPaths = GetAllLearningPaths();

            // pentru dropdown dificultate
            List<string> optionsList = new List<string> {
                    DifficultyLevelsEnum.Easy.ToString(),
                    DifficultyLevelsEnum.Intermediate.ToString(),
                    DifficultyLevelsEnum.Difficult.ToString()
                };

            // Convert List<string> in List<SelectListItem>
            List<SelectListItem> selectListItems = optionsList.Select(option =>
                new SelectListItem { Text = option, Value = option })
                .ToList();

            ViewBag.OptionsSelectList = selectListItems;

            return View(exercise);
        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        public IActionResult Edit(int id, LockedExercise requestedExercise)
        {
            LockedExercise exercise = db.LockedExercises.Find(id);

            var sanitizer = new HtmlSanitizer();

            if (requestedExercise.TestCases != null && !HasValidStructure(requestedExercise.TestCases))
            {
                ModelState.AddModelError("TestCases", "The test cases have an invalid format!");
            }

            if (ModelState.IsValid)
            {
                //nu permiterm modificarea exercitiului decat de admin sau de autorul lui
                if (exercise.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {
                    //preluam noile informatii si protejam de cross-scripting
                    exercise.Title = requestedExercise.Title;
                    exercise.Description = requestedExercise.Description;
                    exercise.Summary = requestedExercise.Summary;
                    exercise.LearningPathId = requestedExercise.LearningPathId;
                    exercise.Restrictions = sanitizer.Sanitize(requestedExercise.Restrictions);
                    exercise.Examples = sanitizer.Sanitize(requestedExercise.Examples);
                    exercise.Difficulty = requestedExercise.Difficulty;
                    exercise.TestCases = requestedExercise.TestCases;

                    db.SaveChanges();

                    TempData["message"] = "The exercise has been modified!";
                    TempData["messageType"] = "alert-success";
                    return Redirect("/LearningPaths/Show/" + requestedExercise.LearningPathId);
                }
                else
                {
                    TempData["message"] = "You're unable to modify an exercise you didn't add!";
                    TempData["messageType"] = "alert-danger";
                    return Redirect("/LearningPaths/Show/" + requestedExercise.LearningPathId);
                }

            }
            else
            {
                // pentru dropdown paths
                exercise.LearningPaths = GetAllLearningPaths();

                // pentru dropdown dificultate
                List<string> optionsList = new List<string> {
                    DifficultyLevelsEnum.Easy.ToString(),
                    DifficultyLevelsEnum.Intermediate.ToString(),
                    DifficultyLevelsEnum.Difficult.ToString()
                };

                // convertim List<string> in List<SelectListItem>
                List<SelectListItem> selectListItems = optionsList.Select(option =>
                    new SelectListItem { Text = option, Value = option })
                    .ToList();

                ViewBag.OptionsSelectList = selectListItems;

                return View(exercise);
            }


        }

        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            LockedExercise exercise = db.LockedExercises.Include("LockedSolutions")
                                            .Where(exercise => exercise.Id == id)
                                            .First();

            // pentru a ne intoarce la pagina cu exercitiile din categoria exercitiului sters
            int? categoryId = exercise.LearningPathId;

            //nu permiterm stergerea exercitiului decat de admin sau de autorul lui
            if (exercise.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {

                db.LockedExercises.Remove(exercise);
                db.SaveChanges();


                TempData["message"] = "The exercise has been deleted!";
                TempData["messageType"] = "alert-danger";

                return Redirect("/LearningPaths/Show/" + categoryId);
            }
            else
            {
                TempData["message"] = "You're unable to modify an exercise you didn't add!";
                TempData["messageType"] = "alert-danger";
                return Redirect("/LearningPaths/Show/" + categoryId);
            }

        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllLearningPaths()
        {
            // preluam categoriile disponibile pentru dropdown
            var selectList = new List<SelectListItem>();

            var paths = from path in db.LearningPaths
                        select path;

            foreach (var path in paths)
            {
                selectList.Add(new SelectListItem
                {
                    Value = path.Id.ToString(),
                    Text = path.Name
                });
            }

            return selectList;
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteCode(int id, string solution, string language)
        {
            string apiKey = Environment.GetEnvironmentVariable("API_KEY");

            LockedSolution solution1 = new LockedSolution
            {
                LockedExerciseId = id,
                SolutionCode = solution,
                UserId = _userManager.GetUserId(User)
            };

            var testCases = db.LockedExercises.Where(ex => ex.Id == id).First().TestCases;
            var request = new
            {
                id = id,
                solution = solution,
                lang = language,
                test_cases = testCases
            };

            _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
            var jsonRequest = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("http://localhost:8000/execute", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserialize the outer JSON to get the `result` string
                    var outerResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);

                    if (outerResult.ContainsKey("result"))
                    {
                        // Deserialize the `result` string to get the actual result dictionary
                        var innerResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(outerResult["result"]);

                        if (innerResult.ContainsKey("score"))
                        {
                            var score = Convert.ToDouble(innerResult["score"]);
                            solution1.Score = (int?)score;

                            db.Add(solution1);
                            db.SaveChanges();

                            if (solution1.Score == 100)
                            {
                                // reward only if it is the first 100 score solution for the exercise
                                var solutions = db.LockedSolutions.Where(s => s.LockedExerciseId == id && s.UserId == _userManager.GetUserId(User) && s.Score == 100);
                                var alreadySolved = solutions.Count() > 1;
                                //if (!alreadySolved)
                                //{
                                // RewardActivity((int)ActivitiesEnum.SOLVE_EXERCISE);
                                // RewardBadgeForSolvingExercise(id);
                                //}
                            }

                            return Json(new { status = 200, test_results = jsonResponse, score = score });
                        }
                    }
                }
                else
                {
                    // Log the response status and content for debugging
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error response from backend: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error" });
            }
            solution1.Score = 0;
            db.Add(solution1);
            db.SaveChanges();
            return BadRequest(new { error = "Error processing request" });
        }

        public IActionResult GetAllLockedExercises()
        {
            var lockedExercises = _lockedExerciseRepository.GetAllLockedExercises();
            return View(lockedExercises);
        }

        // Noua metodă GetSolutionById
        public IActionResult GetLockedExerciseById(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lockedExercise = _lockedExerciseRepository.GetLockedExerciseById(id);
            if (lockedExercise == null)
            {
                return NotFound();
            }

            return View(lockedExercise);
        }
    }
}
