using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using System.Threading.Tasks;
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

        public async Task UpdateUserRole(ApplicationUser user, string newRoleId)
        {
            var userRole = this.context.UserRoles
                .FirstOrDefault(x => x.UserId == user.Id);

            if(userRole == null)
            {
                return;
            }

            userRole.RoleId = newRoleId;
            this.context.UserRoles.Update(userRole);
            await this.context.SaveChangesAsync();
        }
    }
}
