using System;
using System.Linq;
using System.Collections.Generic;

using WebSchool.Data;
using WebSchool.Services.Contracts;
using WebSchool.ViewModels.Comment;

namespace WebSchool.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ApplicationDbContext context;

        public CommentsService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<CommentViewModel> GetPostComments(string postId)
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
