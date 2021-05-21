using System;
using System.Collections.Generic;

using WebSchool.ViewModels.Comment;

namespace WebSchool.ViewModels.Post
{
    public class PostViewModel
    {
        public string Id { get; set; }

        public string Creator { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
