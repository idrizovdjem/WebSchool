using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebSchool.Data.Models
{
    public class School
    {
        public School()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Posts = new HashSet<Post>();
            this.Users = new HashSet<ApplicationUser>();
            this.Classes = new HashSet<SchoolClass>();
            this.Subjects = new HashSet<Subject>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

        public virtual ICollection<SchoolClass> Classes { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }
    }
}