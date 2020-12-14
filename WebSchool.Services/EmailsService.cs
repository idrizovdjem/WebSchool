using SendGrid;
using System.Linq;
using WebSchool.Data;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using WebSchool.Services.Contracts;
using Microsoft.Extensions.Configuration;

namespace WebSchool.Services
{
    public class EmailsService : IEmailsService
    {
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;

        public EmailsService(IConfiguration configuration, ApplicationDbContext context)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public bool IsEmailAvailable(string email)
        {
            return !this.context.RegistrationLinks.Any(x => x.To == email);
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
