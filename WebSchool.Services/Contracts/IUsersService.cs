using WebSchool.Common.Enumerations;

namespace WebSchool.Services.Contracts
{
    public interface IUsersService
    {
        GroupRole GetRoleInGroup(string userId, string groupId);

        bool IsUserInGroup(string userId, string groupId);
    }
}
