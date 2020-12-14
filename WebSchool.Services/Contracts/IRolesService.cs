using WebSchool.Data.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebSchool.Services.Contracts
{
    public interface IRolesService
    {
        string GetUserRole(string userId);

        Task<IdentityResult> AddUserToRole(ApplicationUser user, string roleName);

        Task UpdateUserRole(ApplicationUser user, string newRoleId);
    }
}
