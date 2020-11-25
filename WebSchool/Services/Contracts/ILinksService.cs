using WebSchool.Data.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebSchool.Services.Contracts
{
    public interface ILinksService
    {
        Task<IEnumerable<RegistrationLink>> GenerateLinks(string roleName, int count);

        RegistrationLink GetLink(string registrationLinkId);
    }
}
