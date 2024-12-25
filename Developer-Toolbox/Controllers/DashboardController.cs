using Developer_Toolbox.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Developer_Toolbox.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Inject ApplicationDbContext into the controller
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch dashboard statistics
            var dashboardStats = new DashboardStats
            {
                TotalUsers = await GetTotalUsers(),
                UnansweredQuestions = await GetUnansweredQuestions(),
                TopActiveUsers = await GetTopActiveUsers()
            };

            return View(dashboardStats); // Pass statistics to the view
        }

        private async Task<int> GetTotalUsers()
        {
            // Count total users
            return await Task.FromResult(_context.ApplicationUsers.Count());
        }

        private async Task<int> GetUnansweredQuestions()
        {
            // Fetch all questions and answers from the database
            var questions = await _context.Questions.ToListAsync();
            var answers = await _context.Answers.ToListAsync();

            // Find unanswered questions by checking if there is no corresponding answer
            var unansweredQuestions = questions
                .Where(q => !answers.Any(a => a.QuestionId == q.Id))
                .Count();

            return unansweredQuestions;
        }



        private async Task<List<UserStats>> GetTopActiveUsers()
        {
            // Fetch top 5 active users based on the number of questions they posted
            return await Task.FromResult(
                _context.Questions
                    .GroupBy(q => q.UserId)
                    .OrderByDescending(g => g.Count())
                    .Take(5)
                    .Select(g => new UserStats
                    {
                        UserId = g.Key,
                        TotalQuestions = g.Count()
                    })
                    .ToList());
        }
    }

    // Model for dashboard statistics
    public class DashboardStats
    {
        public int TotalUsers { get; set; }
        public int UnansweredQuestions { get; set; }
        public List<UserStats> TopActiveUsers { get; set; } = new();
    }

    // Model for user statistics
    public class UserStats
    {
        public string UserId { get; set; }
        public int TotalQuestions { get; set; }
    }
}
