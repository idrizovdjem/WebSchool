using WebSchool.Data.Models;
using WebSchool.Models.User;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebSchool.Services.Contracts
{
    public interface IUsersService
    {
        Task<bool> Login(string email, string password);

        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);

        Task<IdentityResult> AddUserToRole(ApplicationUser user, string roleName);

        Task<ApplicationUser> GetUser(ClaimsPrincipal user);

        List<string> GetUserWithEmailContains(string email, string signature, string schoolId);

        ApplicationUser GetUserByEmail(string email);

        ICollection<UsersViewModel> GetUsersTable(string schoolId);
    }
}
