using System;
using WebSchool.Data.Models;

namespace WebSchool.Models.Post
{
    public class PostViewModel
    {
        public ApplicationUser Creator { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
