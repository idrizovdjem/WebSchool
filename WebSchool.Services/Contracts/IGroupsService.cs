using System.Threading.Tasks;
using System.Collections.Generic;

using WebSchool.Data.Models;
using WebSchool.ViewModels.Group;
using WebSchool.ViewModels.Users;
using WebSchool.Common.Enumerations;

namespace WebSchool.Services.Contracts
{
    public interface IGroupsService
    {
        GroupSettingsViewModel GetSettings(string groupId);

        GroupViewModel GetGroupContent(string userId, string groupName);

        bool IsGroupNameAvailable(string name);

        bool GroupExists(string id);

        string GetName(string groupId);

        ICollection<string> GetUserGroups(string userId);

        UserViewModel[] GetMembers(string adminId, string groupId);

        Task<Group> CreateAsync(string userId, string name);

        Task AddUserToGroupAsync(string userId, string groupId, GroupRole role);

        Task ChangeNameAsync(GroupSettingsViewModel input);
    }
}
