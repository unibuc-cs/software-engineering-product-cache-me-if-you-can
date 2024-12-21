
namespace Developer_Toolbox.Models
{
    public class EmailSettings
    {
        public EmailProvider Provider { get; set; }

        // MailHog/SMTP settings
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }

        // Gmail settings
        public string GmailAddress { get; set; }
        public string GmailAppPassword { get; set; }

        // Common settings
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}
