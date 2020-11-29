using System.Threading.Tasks;
using WebSchool.Models.Comment;
using System.Collections.Generic;

namespace WebSchool.Services.Contracts
{
    public interface ICommentsService
    {
        Task AddCommentAsync(string postId, string content, string userId);

        ICollection<CommentViewModel> GetComments(string postId);
    }
}
