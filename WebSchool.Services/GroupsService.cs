using System.Linq;

using WebSchool.Data;
using WebSchool.ViewModels.Group;
using System.Collections.Generic;
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

        public bool IsUserInGroup(string userId, string groupId)
        {
            return dbContext.UserGroups
                .Any(ug => ug.UserId == userId && ug.GroupId == groupId);
        }
    }
}
