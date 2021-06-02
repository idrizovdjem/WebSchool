using System;
using System.Linq;
using System.Threading.Tasks;

using WebSchool.Data;
using WebSchool.Data.Models;
using WebSchool.Services.Contracts;
using WebSchool.ViewModels.Comment;

namespace WebSchool.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ApplicationDbContext dbContext;

        public CommentsService(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        public async Task CreateAsync(string postId, string content, string userId)
        {
            var comment = new Comment()
            {
                PostId = postId,
                Content = content,
                CreatedOn = DateTime.UtcNow,
                CreatorId = userId,
                IsDeleted = false
            };

            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();
        }

        public CommentViewModel[] GetPostComments(string postId)
        {
            return this.dbContext.Comments
                .Where(x => x.PostId == postId && x.IsDeleted == false)
                .Select(x => new CommentViewModel()
                {
                    Id = x.Id,
                    Creator = x.Creator.Email,
                    Content = x.Content,
                    CreatedOn = x.CreatedOn,
                })
                .OrderByDescending(x => x.CreatedOn)
                .ToArray();
        }

        public async Task RemoveAllPostCommentsAsync(string postId)
        {
            var comments = dbContext.Comments
                .Where(c => c.PostId == postId)
                .ToArray();

            dbContext.Comments.RemoveRange(comments);
            await dbContext.SaveChangesAsync();
        }
    }
}
