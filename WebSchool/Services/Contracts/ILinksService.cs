using WebSchool.Data.Models;
using System.Threading.Tasks;

namespace WebSchool.Services.Contracts
{
    public interface ILinksService
    {
        Task GenerateLinks(string roleName, int count);

        RegistrationLink GetLink(string registrationLinkId);
    }
}
