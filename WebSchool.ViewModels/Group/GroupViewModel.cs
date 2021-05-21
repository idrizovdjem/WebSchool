using System.Collections.Generic;

using WebSchool.ViewModels.Post;

namespace WebSchool.ViewModels.Group
{
    public class GroupViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<PostViewModel> NewestPosts { get; set; }
    }
}
