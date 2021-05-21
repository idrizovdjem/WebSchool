using System.Linq;

using WebSchool.Data;
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

        public GroupViewModel GetGroupContent(string groupName)
        {
            return dbContext.Groups
                .Where(g => g.Name == groupName)
                .Select(g => new GroupViewModel()
                {
                    Name = g.Name,
                    NewestPosts = postsService.GetNewestPosts(g.Id, 10)
                })
                .FirstOrDefault();
        }
    }
}
