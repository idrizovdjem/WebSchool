using WebSchool.Data.Models;
using WebSchool.Models.User;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;

namespace WebSchool.Services.Contracts
{
    public interface IUsersService
    {
        Task<bool> Login(string email, string password);

        ApplicationUser GetUserByEmail(string email);

        ApplicationUser GetUserById(string id);

        Task<ApplicationUser> GetUser(ClaimsPrincipal user);

        ICollection<UsersViewModel> GetUsersTable(string schoolId);

        UsersViewModel GetUserForEdit(string id);

        Task UpdateUser(UsersViewModel user);
    }
}
