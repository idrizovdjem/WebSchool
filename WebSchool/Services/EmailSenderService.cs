using System.Threading.Tasks;
using WebSchool.Services.Contracts;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace WebSchool.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration configuration;

        public EmailSenderService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendRegistrationEmail(string registrationId, string recieverEmail)
        {
            var apiKey = this.configuration["SendGridApi"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("idrizovdjem@gmail.com", "WebSchoolAdmin");
            var subject = "WebSchool registration link";
            var to = new EmailAddress(recieverEmail, "Example User");
            var plainTextContent = $"https://localhost:44349/Users/Register?registrationLinkId={registrationId}";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, string.Empty);
            await client.SendEmailAsync(msg);
        }
    }
}
