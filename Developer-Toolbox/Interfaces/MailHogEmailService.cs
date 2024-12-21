using Developer_Toolbox.Models;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace Developer_Toolbox.Interfaces
{
    public class MailHogEmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<IEmailService> _logger;
        private readonly SmtpClient _smtpClient;

        public MailHogEmailService(IOptions<EmailSettings> settings, ILogger<IEmailService> logger)
        {
            _settings = settings.Value;
            _logger = logger;

            _smtpClient = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort)
            {
                EnableSsl = false,  // MailHog doesn't need SSL
                UseDefaultCredentials = true
            };
        }

        private async Task SendEmailAsync(string to, string subject, string htmlBody)
        {
            using var message = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            message.To.Add(to);

            try
            {
                await _smtpClient.SendMailAsync(message);
                _logger.LogInformation("Email sent successfully to {Email}", to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", to);
                throw;
            }
        }

        public async Task SendBadgeAwardedEmailAsync(string userEmail, string userName, string badgeName)
        {
            var subject = $"Congratulations! You've earned the {badgeName} badge!";
            var htmlBody = $@"
            <h2>Congratulations {userName}!</h2>
            <p>You've earned the <strong>{badgeName}</strong> badge on our coding platform.</p>
            <p>Keep up the great work and continue developing your coding skills!</p>
            <br>
            <p>Best regards,</p>
            <p>Your Coding Platform Team</p>";

            await SendEmailAsync(userEmail, subject, htmlBody);
        }

        public async Task SendChallengeStartedEmailAsync(string userEmail, string userName, string challengeName, DateTime endDate)
        {
            var subject = $"New Coding Challenge: {challengeName} Has Started!";
            var htmlBody = $@"
            <h2>Hello {userName}!</h2>
            <p>A new coding challenge has just started: <strong>{challengeName}</strong></p>
            <p>The challenge will end on: {endDate:dddd, MMMM dd, yyyy at HH:mm}</p>
            <p>Login now to participate and test your skills!</p>
            <br>
            <p>Good luck!</p>
            <p>Your Coding Platform Team</p>";

            await SendEmailAsync(userEmail, subject, htmlBody);
        }

        public async Task SendChallengeEndedEmailAsync(string userEmail, string userName, string challengeName)
        {
            var subject = $"Coding Challenge: {challengeName} Has Ended";
            var htmlBody = $@"
            <h2>Hello {userName}!</h2>
            <p>The coding challenge <strong>{challengeName}</strong> has ended.</p>
            <p>Login to see your results and compare them with other participants!</p>
            <br>
            <p>Keep coding!</p>
            <p>Your Coding Platform Team</p>";

            await SendEmailAsync(userEmail, subject, htmlBody);
        }
    }
}
