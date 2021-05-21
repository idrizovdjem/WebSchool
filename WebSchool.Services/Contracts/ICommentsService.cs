using System.Collections.Generic;

using WebSchool.ViewModels.Comment;

namespace WebSchool.Services.Contracts
{
    public interface ICommentsService
    {
        IEnumerable<CommentViewModel> GetPostComments(string postId);
    }
}
