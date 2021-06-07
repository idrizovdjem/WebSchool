using WebSchool.Common.Enumerations;

namespace WebSchool.Services.Common
{
    public interface IUsersService
    {
        GroupRole GetRoleInGroup(string userId, string groupId);

        bool IsUserInGroup(string userId, string groupId);

        bool IsUserGroupCreator(string userId, string groupId);

        bool ValidatePostRemove(string userId, string postId);

        bool ValidateCommentRemove(string userId, string commentId);
    }
}
