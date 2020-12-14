using System.Linq;
using WebSchool.Data;
using WebSchool.ViewModels.User;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using WebSchool.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace WebSchool.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext context;
        private readonly IRolesService rolesService;
        private readonly ILinksService linksService;

        public UsersService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IRolesService rolesService,
            ILinksService linksService)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.rolesService = rolesService;
            this.linksService = linksService;
        }

        public async Task<bool> Login(string email, string password)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            var result = this.signInManager.PasswordSignInAsync(user, password, false, false);
            return result.Result.Succeeded;
        }

        public ApplicationUser GetUserById(string id)
        {
            return this.context.Users.FirstOrDefault(x => x.Id == id);
        }

        public ApplicationUser GetUserByEmail(string email)
        {
            return this.context.Users.FirstOrDefault(x => x.Email == email);
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


        public async Task UpdateUser(UsersViewModel user)
        {
            var oldUser = this.context.Users
                .FirstOrDefault(x => x.Id == user.Id);
            if (oldUser == null)
            {
                return;
            }

            oldUser.FirstName = user.FirstName;
            oldUser.LastName = user.LastName;
            oldUser.Email = user.Email;

            var oldRole = await this.userManager.GetRolesAsync(oldUser);
            if (oldRole.First() != user.Role)
            {
                await this.linksService.UpdateLinkRole(oldUser.Email, user.Role);
                await this.userManager.RemoveFromRoleAsync(oldUser, oldRole.First());
                await this.rolesService.AddUserToRole(oldUser, user.Role);
            }

            this.context.Users.Update(oldUser);
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

        public async Task<ApplicationUser> GetUser(ClaimsPrincipal user)
        {
            return await this.userManager.GetUserAsync(user);
        }
    }
}
