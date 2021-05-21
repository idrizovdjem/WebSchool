using WebSchool.ViewModels.Post;

namespace WebSchool.ViewModels.Group
{
    public class GroupViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public PostViewModel[] NewestPosts { get; set; }
    }
}
