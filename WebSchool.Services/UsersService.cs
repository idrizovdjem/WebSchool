using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebSchool.ViewModels.User;
using WebSchool.Services.Contracts;

namespace WebSchool.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext context;
        private readonly IRolesService rolesService;
        private readonly ILinksService linksService;

        public UsersService(
            ApplicationDbContext context,
            IRolesService rolesService,
            ILinksService linksService)
        {
            this.context = context;
            this.rolesService = rolesService;
            this.linksService = linksService;
        }

        public ApplicationUser GetUserByEmail(string email)
        {
            return this.context.Users
                .FirstOrDefault(x => x.Email == email);
        }

        public ApplicationUser GetUserById(string id)
        {
            return this.context.Users.FirstOrDefault(x => x.Id == id);
        }

        public UsersViewModel GetUserForEdit(string id)
        {
            return this.context.Users
                .Where(x => x.Id == id)
                .Select(x => new UsersViewModel()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Role = this.rolesService.GetUserRole(x.Id)
                })
                .FirstOrDefault();
        }

        public async Task UpdateUser(UsersViewModel userModel)
        {
            var user = this.context.Users
                .FirstOrDefault(x => x.Id == userModel.Id);
            if (user == null)
            {
                return;
            }

            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.Email = userModel.Email;

            var oldRole = this.context.UserRoles
                .FirstOrDefault(x => x.UserId == user.Id);

            var realOldRole = this.context.Roles
                .FirstOrDefault(x => x.Id == oldRole.RoleId);

            if (realOldRole.Name != userModel.Role)
            {
                await this.linksService.UpdateLinkRole(user.Email, userModel.Role);

                var newRole = this.context.Roles
                    .FirstOrDefault(x => x.Name == userModel.Role);
                await this.rolesService.UpdateUserRole(user, newRole.Id);
            }

            this.context.Users.Update(user);
            await this.context.SaveChangesAsync();
        }

        public ICollection<UsersViewModel> GetUsersTable(string schoolId)
        {
            return this.context.Users
                .Where(x => x.SchoolId == schoolId)
                .Select(x => new UsersViewModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Role = this.rolesService.GetUserRole(x.Id)
                })
                .ToList()
                .Where(x => x.Role != "Admin")
                .ToList();
        }
    }
}
