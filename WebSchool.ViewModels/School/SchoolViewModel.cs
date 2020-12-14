using WebSchool.ViewModels.Post;
using System.Collections.Generic;

namespace WebSchool.ViewModels.School
{
    public class SchoolViewModel
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public int Page { get; set; }

        public int MaxPages { get; set; }

        public ICollection<PostViewModel> Posts { get; set; }
    }
}
