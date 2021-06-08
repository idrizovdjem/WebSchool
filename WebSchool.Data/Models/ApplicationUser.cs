using System;
using System.Collections.Generic;

using WebSchool.Data.BaseModels;

namespace WebSchool.Data.Models
{
    public class ApplicationUser : BaseUserModel
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
            Posts = new HashSet<Post>();
            Comments = new HashSet<Comment>();
            Groups = new HashSet<UserGroup>();
            Applications = new HashSet<Application>();
            AssignmentResults = new HashSet<AssignmentResult>();
        }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<UserGroup> Groups { get; set; }

        public virtual ICollection<Application> Applications { get; set; }

        public virtual ICollection<AssignmentResult> AssignmentResults { get; set; }
    }
}
