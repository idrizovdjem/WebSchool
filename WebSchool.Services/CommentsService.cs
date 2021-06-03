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

        public int GetCount(string postId)
        {
            return dbContext.Comments
                .Count(x => x.PostId == postId && x.IsDeleted == false);
        }

        public CommentViewModel[] GetPostComments(string userId, string postId)
        {
            return this.dbContext.Comments
                .Where(x => x.PostId == postId && x.IsDeleted == false)
                .Select(x => new CommentViewModel()
                {
                    Id = x.Id,
                    Creator = x.Creator.Email,
                    Content = x.Content,
                    CreatedOn = x.CreatedOn,
                    IsCreator = x.CreatorId == userId
                })
                .OrderByDescending(x => x.CreatedOn)
                .ToArray();
        }

        public async Task RemoveAllPostCommentsAsync(string postId)
        {
            var comments = dbContext.Comments
                .Where(c => c.PostId == postId)
                .ToList();

            foreach(var comment in comments)
            {
                comment.IsDeleted = true;
                comment.DeletedOn = DateTime.UtcNow;
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> RemoveAsync(string userId, int commentId)
        {
            var comment = dbContext.Comments
                .FirstOrDefault(c => c.CreatorId == userId && c.Id == commentId);

            if(comment == null)
            {
                return false;
            }

            comment.IsDeleted = true;
            comment.DeletedOn = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
