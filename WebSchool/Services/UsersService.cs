using System.Linq;
using WebSchool.Data;
using WebSchool.Models.User;
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

        public UsersService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, IRolesService rolesService)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.rolesService = rolesService;
        }

        public UsersViewModel GetUserEdit(string id)
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

        public ApplicationUser GetUserById(string id)
        {
            return this.context.Users.FirstOrDefault(x => x.Id == id);
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
                await this.userManager.RemoveFromRoleAsync(oldUser, oldRole.First());
                await this.AddUserToRole(oldUser, user.Role);
            }

            this.context.Users.Update(oldUser);
            await this.context.SaveChangesAsync();
        }

        public ApplicationUser GetUserByEmail(string email)
        {
            return this.context.Users.FirstOrDefault(x => x.Email == email);
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

        public ICollection<UsersViewModel> GetTeachers(string schoolId)
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
                .Where(x => x.Role == "Teacher")
                .ToList();
        }

        public async Task<ApplicationUser> GetUser(ClaimsPrincipal user)
        {
            return await this.userManager.GetUserAsync(user);
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

        public async Task<IdentityResult> AddUserToRole(ApplicationUser user, string roleName)
        {
            return await this.userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            return await this.userManager.CreateAsync(user, password);
        }

        public List<string> GetUserWithEmailContains(string email, string signature, string schoolId)
        {
            var users = this.context.Users
                .Where(x => x.SchoolId == schoolId && x.Email.Contains(email))
                .ToList();

            var schoolClass = this.context.SchoolClasses
                .FirstOrDefault(x => x.SchoolId == schoolId && x.Signature == x.Signature);

            var filteredUsers = new List<string>();
            foreach (var user in users)
            {
                if (this.context.UserClasses.Any(x => x.UserId == user.Id))
                {
                    continue;
                }

                filteredUsers.Add(user.Email);
            }

            return filteredUsers;
        }
    }
}
