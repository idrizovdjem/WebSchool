using WebSchool.ViewModels.Post;
using WebSchool.Common.Enumerations;

namespace WebSchool.ViewModels.Group
{
    public class GroupViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public PostViewModel[] NewestPosts { get; set; }

        public GroupRole UserRole { get; set; }
    }
}
