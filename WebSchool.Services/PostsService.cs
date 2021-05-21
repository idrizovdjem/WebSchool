using System.Linq;
using System.Collections.Generic;

using WebSchool.Data;
using WebSchool.ViewModels.Post;

using WebSchool.Services.Contracts;

namespace WebSchool.Services
{
    public class PostsService : IPostsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ICommentsService commentsService;

        public PostsService(ApplicationDbContext context, ICommentsService commentsService)
        {
            this.dbContext = context;
            this.commentsService = commentsService;
        }

        public IEnumerable<PostViewModel> GetNewestPosts(string groupId, int count = 10)
        {
            return dbContext.Posts
                .Where(p => p.GroupId == groupId && p.IsDeleted == false)
                .Take(count)
                .Select(p => new PostViewModel()
                {
                    Content = p.Content,
                    CreatedOn = p.CreatedOn,
                    Creator = p.Creator.Email,
                    Comments = commentsService.GetPostComments(p.Id)
                })
                .ToList();
        }
    }
}
