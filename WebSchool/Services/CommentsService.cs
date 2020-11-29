using System;
using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using WebSchool.Services.Contracts;
using System.Collections.Generic;
using WebSchool.Models.Comment;

namespace WebSchool.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ApplicationDbContext context;

        public CommentsService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddCommentAsync(string postId, string content, string userId)
        {
            var comment = new Comment()
            {
                PostId = postId,
                Content = content,
                CreatedOn = DateTime.UtcNow,
                CreatorId = userId,
            };

            await this.context.Comments.AddAsync(comment);
            await this.context.SaveChangesAsync();
        }

        public ICollection<CommentViewModel> GetComments(string postId)
        {
            return this.context.Comments
                .Where(x => x.PostId == postId && x.IsDeleted == false)
                .Select(x => new CommentViewModel()
                {
                    Id = x.Id,
                    Creator = x.Creator.Email,
                    Content = x.Content,
                    CreatedOn = x.CreatedOn,
                })
                .OrderByDescending(x => x.CreatedOn)
                .ToList();
        }
    }
}
