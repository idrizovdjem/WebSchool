using WebSchool.Data.Models;
using System.Threading.Tasks;
using WebSchool.ViewModels.User;
using System.Collections.Generic;

namespace WebSchool.Services.Contracts
{
    public interface IUsersService
    {
        ApplicationUser GetUserByEmail(string email);

        ApplicationUser GetUserById(string id);

        // ICollection<UsersViewModel> GetUsersTable(string schoolId);

        UsersViewModel GetUserForEdit(string id);

        // Task UpdateUser(UsersViewModel user);
    }
}
