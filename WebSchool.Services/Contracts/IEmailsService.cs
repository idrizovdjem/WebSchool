using System.Threading.Tasks;

namespace WebSchool.Services.Contracts
{
    public interface IEmailsService
    {
        Task SendRegistrationEmail(string registrationId, string recieverEmail);

        bool IsEmailAvailable(string email);
    }
}
