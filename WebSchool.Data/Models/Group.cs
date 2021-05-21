using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using WebSchool.Data.BaseModels;

namespace WebSchool.Data.Models
{
    public class Group : BaseDeletableModel<string>
    {
        public Group()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Users = new HashSet<UserGroup>();
            this.Posts = new HashSet<Post>();
        }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Range(1, 150)]
        public int MaxParticipants { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<UserGroup> Users { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
