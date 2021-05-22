using System.Threading.Tasks;

using WebSchool.ViewModels.Comment;

namespace WebSchool.Services.Contracts
{
    public interface ICommentsService
    {
        CommentViewModel[] GetPostComments(string postId);

        Task CreateAsync(string postId, string content, string userId);
    }
}
