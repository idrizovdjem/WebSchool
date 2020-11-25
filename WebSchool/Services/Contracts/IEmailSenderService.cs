using System.Threading.Tasks;

namespace WebSchool.Services.Contracts
{
    public interface IEmailSenderService
    {
        Task SendRegistrationEmail(string registrationId, string recieverEmail);
    }
}
