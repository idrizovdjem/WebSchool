using WebSchool.ViewModels.Group;

namespace WebSchool.Services.Contracts
{
    public interface IGroupsService
    {
        GroupViewModel GetGroupContent(string groupName);
    }
}
