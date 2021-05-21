using System.Linq;
using System.Threading.Tasks;

using WebSchool.Data;
using WebSchool.Data.Models;
using WebSchool.Services.Contracts;

namespace WebSchool.Services
{
    public class RolesService : IRolesService
    {
        private readonly ApplicationDbContext context;

        public RolesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public string GetUserRole(string userId)
        {
            var userRole = this.context.UserRoles
                .FirstOrDefault(x => x.UserId == userId);

            if (userRole == null)
            {
                return null;
            }

            return this.context.Roles
                .FirstOrDefault(x => x.Id == userRole.RoleId)
                .Name;
        }

        public async Task RemoveUserFromRole(ApplicationUser user)
        {
            var userRole = this.context.UserRoles
                .FirstOrDefault(x => x.UserId == user.Id);

            if (userRole == null)
            {
                return;
            }

            this.context.UserRoles.Remove(userRole);
            await this.context.SaveChangesAsync();
        }
    }
}
