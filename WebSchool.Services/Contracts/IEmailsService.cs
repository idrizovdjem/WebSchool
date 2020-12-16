using System.Threading.Tasks;

namespace WebSchool.Services.Contracts
{
    public interface IEmailsService
    {
        Task SendRegistrationEmail(string registrationId, string recieverEmail, string apiKey);

        bool IsEmailAvailable(string email);
    }
}
