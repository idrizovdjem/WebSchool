using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using WebSchool.Data.BaseModels;

namespace WebSchool.Data.Models
{
    public class ApplicationUser : BaseUserModel
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Posts = new HashSet<Post>();
            this.Comments = new HashSet<Comment>();
            this.Groups = new HashSet<UserGroup>();
        }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<UserGroup> Groups { get; set; }
    }
}
