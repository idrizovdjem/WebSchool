using System.Linq;
using WebSchool.Data;
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

            return this.context.Roles
                .FirstOrDefault(x => x.Id == userRole.RoleId)
                .Name;
        }
    }
}
