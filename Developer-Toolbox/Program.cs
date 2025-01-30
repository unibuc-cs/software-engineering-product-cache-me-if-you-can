using Developer_Toolbox.Controllers;
using Developer_Toolbox.Data;
using Developer_Toolbox.Interfaces;
using Developer_Toolbox.Models;
using Developer_Toolbox.Repositories;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using DotNetEnv;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env
Env.Load();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// PASUL 2 - useri si roluri

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();  // Register HttpClient for compiler

builder.Services.AddScoped<ITagRepository, TagRepository>(); 
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
builder.Services.AddScoped<IReactionRepository, ReactionRepository>();
builder.Services.AddScoped<IBookmarkRepository, BookmarkRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<ISolutionRepository, SolutionRepository>();
builder.Services.AddScoped<IWeeklyChallengeRepository, WeeklyChallengeRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IBadgeRepository, BadgeRepository>();
builder.Services.AddScoped<IRewardBadge, IRewardBadgeImpl>();
builder.Services.AddScoped<IWeeklyChallengeRepository, WeeklyChallengeRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IBadgeRepository, BadgeRepository>();
builder.Services.AddScoped<IRewardActivity, IRewardActivityImpl>();

builder.Services.AddScoped<ILockedExerciseRepository, LockedExerciseRepository>();
builder.Services.AddScoped<ILockedSolutionRepository, LockedSolutionRepository>();

// Hangfire configuration
builder.Services.AddHangfire(config => config
    .UseSqlServerStorage(connectionString)); 

builder.Services.AddHangfireServer();  // Configurarea serverului Hangfire pentru a executa joburi

builder.Services.AddScoped<ChallengeNotificationService>();  // Inregistreaza serviciul pentru verificarea provocarilor


// Email service
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

var emailProvider = builder.Configuration
    .GetValue<EmailProvider>("EmailSettings:Provider");

builder.Services.AddScoped<IEmailService>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<EmailSettings>>();
    var logger = sp.GetRequiredService<ILogger<IEmailService>>();
    return emailProvider switch
    {
        EmailProvider.MailHog => new MailHogEmailService(settings, logger),
        _ => throw new ArgumentException("Invalid email provider")
    };
});

// If you need IEmailSender, register it to use the same instance
builder.Services.AddScoped<IEmailSender>(sp =>
    sp.GetRequiredService<IEmailService>() as IEmailSender);


var app = builder.Build();

// PASUL 5 - useri si roluri
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Hangfire Dashboard pentru vizualizarea joburilor
app.UseHangfireDashboard("/hangfire");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


// Programarea jobului recurent pentru verificarea provocarilor active
RecurringJob.AddOrUpdate<ChallengeNotificationService>(
    service => service.CheckActiveChallengesAndSendNotifications(),
    Cron.Minutely); //Seteaza jobul sa ruleze la fiecare minut


app.Run();
