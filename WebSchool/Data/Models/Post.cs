﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebSchool.Data.Models
{
    public class Post
    {
        public Post()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Comments = new HashSet<Comment>();
        }

        public string Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        [Required]
        public string SchoolId { get; set; }

        public virtual School School { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}