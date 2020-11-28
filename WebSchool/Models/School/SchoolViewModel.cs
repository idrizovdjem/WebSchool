using WebSchool.Models.Post;
using System.Collections.Generic;

namespace WebSchool.Models.School
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
