using WebSchool.Data.Models;
using System.Threading.Tasks;

namespace WebSchool.Services.Contracts
{
    public interface IRolesService
    {
        string GetUserRole(string userId);

        Task RemoveUserFromRole(ApplicationUser user);
    }
}
