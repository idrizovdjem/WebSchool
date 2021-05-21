using WebSchool.ViewModels.Group;

namespace WebSchool.Services.Contracts
{
    public interface IGroupsService
    {
        GroupViewModel GetGroupContent(string groupName);

        bool IsUserInGroup(string userId, string groupId);

        string GetName(string groupId);
    }
}
