using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using WebSchool.Data;
using WebSchool.Data.Models;
using WebSchool.ViewModels.Users;
using WebSchool.ViewModels.Group;
using WebSchool.Services.Contracts;
using WebSchool.Common.Enumerations;

namespace WebSchool.Services
{
    public class GroupsService : IGroupsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IPostsService postsService;
        private readonly IApplicationsService applicationsService;

        public GroupsService(ApplicationDbContext dbContext, IPostsService postsService, IApplicationsService applicationsService)
        {
            this.dbContext = dbContext;
            this.postsService = postsService;
            this.applicationsService = applicationsService;
        }

        public async Task AddUserToGroupAsync(string userId, string groupId, GroupRole role)
        {
            var userRole = dbContext.Roles
                .FirstOrDefault(r => r.Name.ToLower() == role.ToString().ToLower());

            var userGroup = new UserGroup()
            {
                UserId = userId,
                GroupId = groupId,
                Role = userRole
            };

            await dbContext.UserGroups.AddAsync(userGroup);
            await dbContext.SaveChangesAsync();
        }

        public async Task ChangeNameAsync(GroupSettingsViewModel input)
        {
            var group = dbContext.Groups
                .Find(input.Id);

            group.Name = input.Name.Trim();
            await dbContext.SaveChangesAsync();
        }

        public async Task<Group> CreateAsync(string userId, string name)
        {
            var group = new Group()
            {
                Name = name.Trim(),
                OwnerId = userId,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false
            };

            await dbContext.Groups.AddAsync(group);
            await dbContext.SaveChangesAsync();

            await AddUserToGroupAsync(userId, group.Id, GroupRole.Admin);

            return group;
        }

        public GroupViewModel GetGroupContent(string userId, string groupName)
        {
            var groupViewModel = dbContext.Groups
                .Where(g => g.Name == groupName)
                .Select(g => new GroupViewModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    NewestPosts = postsService.GetNewestPosts(g.Id, 10)
                })
                .FirstOrDefault();

            if(groupName != "Global Group")
            {
                var roleId = dbContext.UserGroups
                .First(ug => ug.UserId == userId && ug.GroupId == groupViewModel.Id).RoleId;

                var role = dbContext.Roles
                    .Find(roleId).Name;

                groupViewModel.UserRole = (GroupRole)Enum.Parse(typeof(GroupRole), role);
            }
            else
            {
                groupViewModel.UserRole = GroupRole.Student;
            }

            return groupViewModel;
        }

        public BrowseGroupViewModel[] GetGroupsContainingName(string userId, string groupName)
        {
            var groups = dbContext.Groups
                .Where(g => 
                    g.Name.Contains(groupName) && 
                    g.IsDeleted == false && 
                    g.Name != "Global Group")
                .Select(g => new BrowseGroupViewModel()
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToArray();

            foreach(var group in groups)
            {
                if (IsUserInGroup(userId, group.Id))
                {
                    group.RequestStatus = ApplicationStatus.InGroup.ToString();
                }
                else
                {
                    var applicationStatus = applicationsService.GetApplicationStatus(userId, group.Id);
                    group.RequestStatus = applicationStatus.ToString();
                }
            }

            return groups;
        }

        public UserViewModel[] GetMembers(string adminId, string groupId)
        {
            return dbContext.UserGroups
                .Where(ug => ug.GroupId == groupId && ug.UserId != adminId)
                .Select(ug => new UserViewModel()
                {
                    Id = ug.User.Id,
                    Email = ug.User.Email
                })
                .ToArray();
        }

        public string GetName(string groupId)
        {
            return dbContext.Groups
                .FirstOrDefault(g => g.Id == groupId)?.Name;
        }

        public GroupSettingsViewModel GetSettings(string groupId)
        {
            return dbContext.Groups
                .Where(g => g.Id == groupId)
                .Select(g => new GroupSettingsViewModel()
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .FirstOrDefault();
        }

        public ICollection<string> GetUserGroups(string userId)
        {
            var groupNames =  dbContext.UserGroups
                .Where(ug => ug.UserId == userId)
                .Select(ug => ug.Group.Name)
                .ToList();

            groupNames.Add("Global Group");
            return groupNames;
        }

        public bool GroupExists(string id)
        {
            return dbContext.Groups.Any(g => g.Id == id);
        }

        public bool IsGroupNameAvailable(string name)
        {
            return dbContext.Groups.All(g => g.Name != name);
        }

        public bool IsUserInGroup(string userId, string groupId)
        {
            return dbContext.UserGroups
                .Any(ug => ug.UserId == userId && ug.GroupId == groupId);
        }
    }
}
