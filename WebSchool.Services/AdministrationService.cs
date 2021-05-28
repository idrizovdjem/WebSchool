using WebSchool.Services.Contracts;
using WebSchool.Common.Enumerations;

namespace WebSchool.Services
{
    public class AdministrationService : IAdministrationService
    {
        private readonly IGroupsService groupsService;
        private readonly IUsersService usersService;

        public AdministrationService(IGroupsService groupsService, IUsersService usersService)
        {
            this.groupsService = groupsService;
            this.usersService = usersService;
        }

        public bool ValidateIfUserIsAdmin(string userId, string groupId)
        {
            if (groupsService.IsUserInGroup(userId, groupId) == false)
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
