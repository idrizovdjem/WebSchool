using System;

namespace WebSchool.ViewModels.Post
{
    public class AdministrationPostViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Creator { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CommentsCount { get; set; }
    }
}
