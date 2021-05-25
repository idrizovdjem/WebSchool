using System.Threading.Tasks;
using System.Collections.Generic;

using WebSchool.Data.Models;
using WebSchool.ViewModels.Group;

namespace WebSchool.Services.Contracts
{
    public interface IGroupsService
    {
        GroupViewModel GetGroupContent(string groupName);

        bool IsUserInGroup(string userId, string groupId);

        bool IsGroupNameAvailable(string name);

        string GetName(string groupId);

        string[] GetGroupsContainingName(string userId, string groupName);

        ICollection<string> GetUserGroups(string userId);

        Task<Group> CreateAsync(string userId, string name);

        Task AddUserToGroup(string userId, string groupId, string role);
    }
}
