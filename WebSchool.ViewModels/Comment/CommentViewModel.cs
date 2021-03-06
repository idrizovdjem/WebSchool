﻿using System;

namespace WebSchool.ViewModels.Comment
{
    public class CommentViewModel
    {
        public string Id { get; set; }

        public string Creator { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsCreator { get; set; }
    }
}
