using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using WebSchool.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace WebSchool.Services
{
    public class RolesService : IRolesService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public RolesService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IdentityResult> AddUserToRole(ApplicationUser user, string roleName)
        {
            return await this.userManager.AddToRoleAsync(user, roleName);
        }

        public string GetUserRole(string userId)
        {
            var userRole = this.context.UserRoles
                .FirstOrDefault(x => x.UserId == userId);

            return this.context.Roles
                .FirstOrDefault(x => x.Id == userRole.RoleId)
                .Name;
        }
    }
}
