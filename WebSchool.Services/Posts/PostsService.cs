using System;
using System.Linq;
using System.Threading.Tasks;

using WebSchool.Data;
using WebSchool.Data.Models;
using WebSchool.ViewModels.Post;
using WebSchool.Common.Constants;

namespace WebSchool.Services.Posts
{
    public class PostsService : IPostsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ICommentsService commentsService;

        public PostsService(
            ApplicationDbContext context, 
            ICommentsService commentsService)
        {
            this.dbContext = context;
            this.commentsService = commentsService;
        }

        public async Task CreateAsync(CreatePostInputModel input, string userId)
        {
            var post = new Post()
            {
                Content = input.Content,
                Title = input.Title,
                GroupId = input.GroupId,
                CreatorId = userId,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false
            };

            await dbContext.Posts.AddAsync(post);
            await dbContext.SaveChangesAsync();
        }

        public async Task EditAsync(EditPostInputModel input)
        {
            var post = dbContext.Posts
                .FirstOrDefault(p => p.Id == input.Id);

            if(post == null)
            {
                return;
            }

            post.Title = input.Title;
            post.Content = input.Content;

            await dbContext.SaveChangesAsync();
        }

        public AdministrationPostViewModel[] GetAll(string groupId)
        {
            return dbContext.Posts
                .Where(p => p.GroupId == groupId && p.IsDeleted == false)
                .OrderByDescending(p => p.CreatedOn)
                .Select(p => new AdministrationPostViewModel()
                {
                    Id = p.Id,
                    Title = p.Title.Length < 50 ? p.Title : p.Title.Substring(0, 50) + "...",
                    Creator = p.Creator.Email,
                    CreatedOn = p.CreatedOn,
                    Content = p.Content.Substring(0, 25) + "...",
                    CommentsCount = commentsService.GetCount(p.Id)
                })
                .ToArray();
        }

        public PostViewModel GetById(string userId, string postId)
        {
            return dbContext.Posts
                .Where(p => p.Id == postId && p.IsDeleted == false)
                .Select(p => new PostViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Creator = p.Creator.Email,
                    CreatedOn = p.CreatedOn,
                    Content = p.Content,
                    Comments = commentsService.GetPostComments(userId, p.Id),
                    IsCreator = p.CreatorId == userId
                })
                .FirstOrDefault();
        }

        public int GetCount(string groupId)
        {
            return dbContext.Posts
                .Count(p => p.GroupId == groupId && p.IsDeleted == false);
        }

        public EditPostInputModel GetForEdit(string userId, string postId)
        {
            return dbContext.Posts
                .Where(p => p.Id == postId && p.CreatorId == userId && p.IsDeleted == false)
                .Select(p => new EditPostInputModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content
                })
                .FirstOrDefault();
        }

        public PostViewModel[] GetNewestPosts(string userId, string groupId, int count = PostConstants.MostPopularPostsCount)
        {
            return dbContext.Posts
                .Where(p => p.GroupId == groupId && p.IsDeleted == false)
                .OrderByDescending(p => p.CreatedOn)
                .Take(count)
                .Select(p => new PostViewModel()
                {
                    Id = p.Id,
                    Title = p.Title.Length < PostConstants.MaximumTitlePreviewLength ? p.Title : p.Title.Substring(0, PostConstants.MaximumTitlePreviewLength) + "...",
                    Content = p.Content.Length < PostConstants.MaximumContentPreviewLength ? p.Content : p.Content.Substring(0, PostConstants.MaximumContentPreviewLength) + "...",
                    CreatedOn = p.CreatedOn,
                    Creator = p.Creator.Email,
                    IsCreator = p.CreatorId == userId,
                    Comments = commentsService.GetPostComments(userId, p.Id)
                })
                .ToArray();
        }

        public async Task<bool> RemoveAsync(string id)
        {
            var post = dbContext.Posts
                .FirstOrDefault(p => p.Id == id);

            if(post == null)
            {
                return false;
            }

            await commentsService.RemoveAllPostCommentsAsync(id);

            post.IsDeleted = true;
            post.DeletedOn = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
