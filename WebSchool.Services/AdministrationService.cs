using WebSchool.Services.Contracts;
using WebSchool.Common.Enumerations;

namespace WebSchool.Services
{
    public class AdministrationService : IAdministrationService
    {
        private readonly IUsersService usersService;

        public AdministrationService(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public bool ValidateIfUserIsAdmin(string userId, string groupId)
        {
            if (usersService.IsUserInGroup(userId, groupId) == false)
            {
                return false;
            }

            var userRole = usersService.GetRoleInGroup(userId, groupId);
            if (userRole != GroupRole.Admin)
            {
                return false;
            }

            return true;
        }
    }
}
