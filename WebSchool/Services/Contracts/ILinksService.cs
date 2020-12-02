using WebSchool.Data.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebSchool.Models.RegistrationLink;

namespace WebSchool.Services.Contracts
{
    public interface ILinksService
    {
        Task<RegistrationLink> GenerateAdminLink(string email);

        Task<IEnumerable<RegistrationLink>> GenerateLinks(string roleName, string from, string schoolId, string[] toEmails);

        RegistrationLink GetLink(string registrationLinkId);

        Task UseLink(string registrationLinkId);

        bool IsRoleValid(string role);

        ICollection<RegistrationLinkViewModel> GetGeneratedLinks(string adminId);

        Task Delete(string id);
    }
}
