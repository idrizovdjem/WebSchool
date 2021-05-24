using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using WebSchool.Data;
using WebSchool.Data.Models;
using WebSchool.ViewModels.Group;
using WebSchool.Services.Contracts;

namespace WebSchool.Services
{
    public class GroupsService : IGroupsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IPostsService postsService;

        public GroupsService(ApplicationDbContext dbContext, IPostsService postsService)
        {
            this.dbContext = dbContext;
            this.postsService = postsService;
        }

        public async Task AddUserToGroup(string userId, string groupId, string role)
        {
            var userRole = dbContext.Roles
                .Where(r => r.Name.ToLower() == role.ToLower())
                .FirstOrDefault();

            var userGroup = new UserGroup()
            {
                UserId = userId,
                GroupId = groupId,
                Role = userRole
            };

            await dbContext.UserGroups.AddAsync(userGroup);
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

            await AddUserToGroup(userId, group.Id, "admin");

            return group;
        }

        public GroupViewModel GetGroupContent(string groupName)
        {
            return dbContext.Groups
                .Where(g => g.Name == groupName)
                .Select(g => new GroupViewModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    NewestPosts = postsService.GetNewestPosts(g.Id, 10)
                })
                .FirstOrDefault();
        }

        public string GetName(string groupId)
        {
            return dbContext.Groups
                .FirstOrDefault(g => g.Id == groupId)?.Name;
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
