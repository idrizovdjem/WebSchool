using System;
using WebSchool.Data.Models;

namespace WebSchool.Models.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public ApplicationUser Creator { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
