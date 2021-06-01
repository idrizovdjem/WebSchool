using System;
using System.Linq;
using System.Threading.Tasks;

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

        public PostViewModel[] GetAll(string groupId)
        {
            return dbContext.Posts
                .Where(p => p.GroupId == groupId && p.IsDeleted == false)
                .Select(p => new PostViewModel()
                {
                    Id = p.Id,
                    Creator = p.Creator.Email,
                    CreatedOn = p.CreatedOn,
                    Content = p.Content.Substring(0, 25) + "...",
                    CommentsCount = p.Comments.Count
                })
                .ToArray();
        }

        public PostPreviewViewModel GetById(string postId)
        {
            return dbContext.Posts
                .Where(p => p.Id == postId && p.IsDeleted == false)
                .Select(p => new PostPreviewViewModel()
                {
                    Id = p.Id,
                    Creator = p.Creator.Email,
                    Content = p.Content,
                    Comments = commentsService.GetPostComments(p.Id),
                    CreatedOn = p.CreatedOn
                })
                .FirstOrDefault();
        }

        public PostViewModel[] GetNewestPosts(string userId, string groupId, int count = 10)
        {
            return dbContext.Posts
                .Where(p => p.GroupId == groupId && p.IsDeleted == false)
                .OrderByDescending(p => p.CreatedOn)
                .Take(count)
                .Select(p => new PostViewModel()
                {
                    Id = p.Id,
                    Title = p.Title.Length < 50 ? p.Title : p.Title.Substring(0, 50) + "...",
                    Content = p.Content.Length < 500 ? p.Content : p.Content.Substring(0, 500) + "...",
                    CreatedOn = p.CreatedOn,
                    Creator = p.Creator.Email,
                    CommentsCount = p.Comments.Count,
                    IsCreator = p.CreatorId == userId
                })
                .ToArray();
        }
    }
}
