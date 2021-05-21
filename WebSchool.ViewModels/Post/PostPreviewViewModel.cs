using WebSchool.ViewModels.Comment;

namespace WebSchool.ViewModels.Post
{
    public class PostPreviewViewModel
    {
        public string Id { get; set; }

        public string Creator { get; set; }

        public string Content { get; set; }

        public CommentViewModel[] Comments { get; set; }
    }
}
