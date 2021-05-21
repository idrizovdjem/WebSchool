using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using WebSchool.Data;
using WebSchool.ViewModels.Post;

using WebSchool.Data.Models;
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

        public async Task CreateAsync(CreatePostInputModel input, string userId)
        {
            var post = new Post()
            {
                Content = input.Content,
                GroupId = input.GroupId,
                CreatorId = userId,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false
            };

            await dbContext.Posts.AddAsync(post);
            await dbContext.SaveChangesAsync();
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
