using System.Threading.Tasks;
using System.Collections.Generic;

using WebSchool.Data.Models;
using WebSchool.ViewModels.Group;
using WebSchool.Common.Enumerations;

namespace WebSchool.Services.Groups
{
    public interface IGroupsService
    {
        string GetIdByName(string name);

        GroupViewModel GetGroupContent(string userId, string groupId);

        bool IsGroupNameAvailable(string name);

        bool GroupExists(string id);

        string GetName(string groupId);

        GroupItemViewModel[] GetUserGroups(string userId);

        Task<string> CreateAsync(string userId, string name);

        Task AddUserToGroupAsync(string userId, string groupId, GroupRole role);

        Task ChangeNameAsync(ChangeGroupNameInputModel input);
    }
}
