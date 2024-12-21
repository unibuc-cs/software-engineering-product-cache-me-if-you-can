using Developer_Toolbox.Models;

namespace Developer_Toolbox.Interfaces
{
    public interface IEmailService
    {
        Task SendBadgeAwardedEmailAsync(string userEmail, string userName, Badge badge);
/*        Task SendNewChallengeEmailAsync(string userEmail, string userName, WeeklyChallenge challenge);*/
        Task SendAnsweredReceivedEmailAsync(string userEmail, string userName, Question question);
        Task SendContentDeletedByAdminEmailAsync(string userEmail, string userName, string deletedContent);
    }
}
