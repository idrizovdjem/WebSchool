using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using WebSchool.Data;
using WebSchool.Data.Models;
using WebSchool.Services.Posts;
using WebSchool.ViewModels.Group;
using WebSchool.Common.Enumerations;

namespace WebSchool.Services.Groups
{
    public class GroupsService : IGroupsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IPostsService postsService;

        public GroupsService(
            ApplicationDbContext dbContext, 
            IPostsService postsService)
        {
            this.dbContext = dbContext;
            this.postsService = postsService;
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

        public async Task ChangeNameAsync(ChangeGroupNameInputModel input)
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

        public GroupViewModel GetGroupContent(string userId, string groupId)
        {
            var groupViewModel = dbContext.Groups
                .Where(g => g.Id == groupId)
                .Select(g => new GroupViewModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    NewestPosts = postsService.GetNewestPosts(userId, g.Id, 10)
                })
                .FirstOrDefault();

            if(groupViewModel == null)
            {
                return null;
            }

            if(groupViewModel.Name != "Global Group")
            {
                var roleId = dbContext.UserGroups
                .FirstOrDefault(ug => ug.UserId == userId && ug.GroupId == groupViewModel.Id)?.RoleId;

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

        public string GetIdByName(string name)
        {
            return dbContext.Groups
                .FirstOrDefault(g => g.Name == name)?.Id;
        }

        public string GetName(string groupId)
        {
            return dbContext.Groups
                .FirstOrDefault(g => g.Id == groupId)?.Name;
        }

        public ICollection<NavGroupItemViewModel> GetUserGroups(string userId)
        {
            var groups =  dbContext.UserGroups
                .Where(ug => ug.UserId == userId)
                .Select(ug => new NavGroupItemViewModel()
                {
                    Id = ug.GroupId,
                    Name = ug.Group.Name
                })
                .ToList();

            var globalGroup = dbContext.Groups
                .Select(g => new NavGroupItemViewModel()
                { 
                    Id = g.Id,
                    Name = g.Name
                })
                .First(g => g.Name == "Global Group");

            groups.Add(globalGroup);
            return groups;
        }

        public bool GroupExists(string id)
        {
            return dbContext.Groups.Any(g => g.Id == id);
        }

        public bool IsGroupNameAvailable(string name)
        {
            return dbContext.Groups.All(g => g.Name != name);
        }
    }
}
