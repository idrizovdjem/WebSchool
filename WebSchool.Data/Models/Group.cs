using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using WebSchool.Data.BaseModels;
using WebSchool.Common.Constants;

namespace WebSchool.Data.Models
{
    public class Group : BaseDeletableModel<string>
    {
        public Group()
        {
            Id = Guid.NewGuid().ToString();
            Users = new HashSet<UserGroup>();
            Posts = new HashSet<Post>();
        }

        [Required]
        [MaxLength(GroupConstants.MaximumNameLength)]
        public string Name { get; set; }

        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<UserGroup> Users { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
