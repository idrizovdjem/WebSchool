using WebSchool.ViewModels.Comment;

namespace WebSchool.Services.Contracts
{
    public interface ICommentsService
    {
        CommentViewModel[] GetPostComments(string postId);
    }
}
