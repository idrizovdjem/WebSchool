using System;

namespace WebSchool.Models.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Creator { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
