using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using WebSchool.Data.BaseModels;

namespace WebSchool.Data.Models
{
    public class Post : BaseDeletableModel<string>
    {
        public Post()
        {
            Id = Guid.NewGuid().ToString();
            Comments = new HashSet<Comment>();
        }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        [Required]
        public string GroupId { get; set; }

        public virtual Group Group { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}