using WebSchool.Data.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebSchool.Services.Contracts
{
    public interface ILinksService
    {
        Task<RegistrationLink> GenerateAdminLink(string email);

        Task<IEnumerable<RegistrationLink>> GenerateLinks(string roleName, string from, string[] toEmails);

        RegistrationLink GetLink(string registrationLinkId);

        Task UseLink(string registrationLinkId);
    }
}
