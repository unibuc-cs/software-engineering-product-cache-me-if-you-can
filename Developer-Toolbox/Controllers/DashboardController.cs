using Developer_Toolbox.Data;
using Developer_Toolbox.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;

namespace Developer_Toolbox.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dashboardStats = new DashboardStats
            {
                TotalUsers = await GetTotalUsers(),
                UnansweredQuestions = await GetUnansweredQuestions(),
                TopActiveUsers = await GetTopActiveUsers(),
                UnansweredQuestionsList = await GetQuestionsWithNoAnswers(),
                TotalExerciseCategories = await GetTotalExerciseCategories(),
                EngagedUsers = await GetEngagedUsers(),
                TotalQuestions = await GetTotalQuestions(),
                AnsweredQuestions = await GetAnsweredQuestions(),
                TrendingCategories = await GetTrendingCategories()
            };

            // Calculate non-engaged users (Total - Engaged)
            dashboardStats.NonEngagedUsers = dashboardStats.TotalUsers - dashboardStats.EngagedUsers;

            // Calculate proportions for the donut chart
            dashboardStats.NonEngagedProportion = (double)dashboardStats.NonEngagedUsers / dashboardStats.TotalUsers;
            dashboardStats.EngagedProportion = 1 - dashboardStats.NonEngagedProportion;

            // Serialize TrendingCategories to JSON
            ViewData["TrendingData"] = JsonSerializer.Serialize(
                dashboardStats.TrendingCategories.Select(c => new { c.CategoryName, c.UserCount })
            );
            return View(dashboardStats);
        }

        private async Task<int> GetTotalUsers()
        {
            // Count total users
            return await Task.FromResult(_context.ApplicationUsers.Count());
        }

        private async Task<int> GetUnansweredQuestions()
        {
            // Count unanswered questions
            return await _context.Questions
                .Where(q => !_context.Answers.Any(a => a.QuestionId == q.Id))
                .CountAsync();
        }

        private async Task<List<Question>> GetQuestionsWithNoAnswers()
        {
            // Fetch all questions with no answers
            return await _context.Questions.Include("User")
                .Where(q => !_context.Answers.Any(a => a.QuestionId == q.Id))
                .ToListAsync();
        }

        private async Task<List<UserStats>> GetTopActiveUsers()
        {
            // Group the answers by UserId and count the number of answers for each user
            var userAnswerCounts = await _context.Answers
                .GroupBy(a => a.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    TotalAnswers = g.Count()
                })
                .OrderByDescending(g => g.TotalAnswers)
                .Take(5) // Get top 5 users
                .ToListAsync();

            // Join the result with the ApplicationUser table to get FirstName and LastName
            var topUsers = userAnswerCounts
                .Join(
                    _context.ApplicationUsers,
                    userStats => userStats.UserId,
                    user => user.Id,
                    (userStats, user) => new UserStats
                    {
                        UserId = userStats.UserId,
                        TotalAnswers = userStats.TotalAnswers, // Renaming for display purposes
                        Username = user.UserName ?? "N/A",
                    })
                .ToList();

            return topUsers;
        }

        private async Task<int> GetTotalExerciseCategories()
        {
            // Count the total number of exercise categories
            return await _context.Categories.CountAsync();
        }

        private async Task<int> GetEngagedUsers()
        {
            // Count users who have answered at least one question
            return await _context.ApplicationUsers
                .Where(user => _context.Answers.Any(a => a.UserId == user.Id))
                .CountAsync();
        }

        // Fetch the total number of questions
        private async Task<int> GetTotalQuestions()
        {
            return await _context.Questions.CountAsync();
        }

        // Fetch the total number of answered questions
        private async Task<int> GetAnsweredQuestions()
        {
            return await _context.Questions
                .Where(q => _context.Answers.Any(a => a.QuestionId == q.Id))
                .CountAsync();
        }

        public async Task<List<CategoryStats>> GetTrendingCategories()
        {
            // Fetch the top 5 categories with the most unique users submitting solutions
            var trendingCategories = await _context.Categories
                .Select(category => new CategoryStats
                {
                    CategoryName = category.CategoryName,
                    UserCount = category.Exercises
                        .SelectMany(exercise => exercise.Solutions)
                        .Select(solution => solution.UserId)
                        .Distinct()
                        .Count() // Count unique users per category
                })
                .OrderByDescending(stat => stat.UserCount)
                .Take(5)
                .ToListAsync();

            return trendingCategories;
        }

    }

    // Model for dashboard statistics
    public class DashboardStats
    {
        public int TotalUsers { get; set; }
        public int UnansweredQuestions { get; set; }
        public List<UserStats> TopActiveUsers { get; set; } = new();
        public List<Question> UnansweredQuestionsList { get; set; } = new(); // Add this property
        public int TotalExerciseCategories { get; set; }

        // Removed NonEngagedUsers from the constructor since it's calculated directly
        public int EngagedUsers { get; set; }
        public int NonEngagedUsers { get; set; }

        public double NonEngagedProportion { get; set; }
        public double EngagedProportion { get; set; }

        public int TotalQuestions { get; set; }

        public int AnsweredQuestions { get; set; }

        public List<CategoryStats> TrendingCategories { get; set; } = new();
    }

    // Model for user statistics
    public class UserStats
    {
        public string UserId { get; set; }
        public int TotalAnswers { get; set; }
        public string Username { get; set; }
    }

    // for popular learning topics
    public class CategoryStats
    {
        public string CategoryName { get; set; }
        public int UserCount { get; set; }
    }
}
