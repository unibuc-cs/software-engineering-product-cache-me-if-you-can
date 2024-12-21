namespace Developer_Toolbox.Interfaces
{
    public interface IEmailService
    {
        Task SendBadgeAwardedEmailAsync(string userEmail, string userName, string badgeName);
        Task SendChallengeStartedEmailAsync(string userEmail, string userName, string challengeName, DateTime endDate);
        Task SendChallengeEndedEmailAsync(string userEmail, string userName, string challengeName);
    }
}
