using System;
using WebSchool.Data.Models;
using WebSchool.Models.Comment;
using System.Collections.Generic;

namespace WebSchool.Models.Post
{
    public class PostViewModel
    {
        public string Id { get; set; }

        public ApplicationUser Creator { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }
    }
}
