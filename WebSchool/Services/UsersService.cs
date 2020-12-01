using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using WebSchool.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WebSchool.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext context;

        public UsersService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        public async Task<IdentityResult> AddUserToRole(ApplicationUser user, string roleName)
        {
            return await this.userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            return await this.userManager.CreateAsync(user, password);
        }

        public async Task<ApplicationUser> GetUser(ClaimsPrincipal user)
        {
            return await this.userManager.GetUserAsync(user);
        }

        public ICollection<string> GetUserWithEmailContains(string email, string signature, string schoolId)
        {
            return null;
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
    }
}
