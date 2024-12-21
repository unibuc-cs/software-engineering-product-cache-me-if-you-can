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

        public async Task SendBadgeAwardedEmailAsync(string userEmail, string userName, Badge badge)
        {
            var subject = $"Congratulations! You've earned the {badge.Title} badge!";
            var htmlBody = $@"
            <h2>Congratulations {userName}!</h2>
            <p>You've earned the <strong>{badge.Title}: {badge.Description}</strong> badge on our platform.</p>
            <p>Keep up the great work and continue developing your coding skills!</p>
            <br>
            <p>Best regards,</p>
            <p>Developer Toolbox Team</p>";

            await SendEmailAsync(userEmail, subject, htmlBody);
        }

/*        public async Task SendChallengeStartedEmailAsync(string userEmail, string userName, string challengeName, DateTime endDate)
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
        }*/

        public async Task SendAnsweredReceivedEmailAsync(string userEmail, string userName, Question question)
        {
            var subject = $"You have received a new answer!";
            var htmlBody = $@"
            <h2>Hello {userName}!</h2>
            <p>You have a new answer to your question: </p>
            <br>
            <p><strong>{question.Title}</strong></p>
            <p>{question.Description}</p>
            <br>
            <p>Log in to see the comment you have received!</p>
            <br>
            <p>Developer Toolbox Team</p>";

            await SendEmailAsync(userEmail, subject, htmlBody);
        }

        public async Task SendContentDeletedByAdminEmailAsync(string userEmail, string userName, string deletedContent)
        {
            var subject = $"Some content you posted was deleted by our moderators";
            var htmlBody = $@"
            <h2>Hello {userName}!</h2>
            <p>The moderators on our platform try to keep it a secure and friendly space where everybody can learn and practice coding.</p>
            <p>We write to inform you that the following content you posted on the platform does not comply with our content standards and was removed.</p>
            <br>
            <p>{deletedContent}</p>
            <br>
            <p>Thank you for your understanding.</p>
            <p>Developer Toolbox Team</p>";

            await SendEmailAsync(userEmail, subject, htmlBody);
        }
    }
}
