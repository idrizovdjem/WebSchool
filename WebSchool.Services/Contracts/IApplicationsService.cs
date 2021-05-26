using System.Threading.Tasks;

using WebSchool.Common.Enumerations;

namespace WebSchool.Services.Contracts
{
    public interface IApplicationsService
    {
        ApplicationStatus GetApplicationStatus(string userId, string groupId);

        Task ApplyAsync(string userId, string groupId);
    }
}
