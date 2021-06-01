using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebSchool.Data.Models
{
    public class Assignment
    {
        public Assignment()
        {
            Id = Guid.NewGuid().ToString();
            Students = new HashSet<UserAssignment>();
            Results = new HashSet<AssignmentResult>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public int Points { get; set; }

        [Required]
        public string TeacherId { get; set; }

        public virtual ApplicationUser Teacher { get; set; }

        [Required]
        public string GroupId { get; set; }

        public virtual Group Group { get; set; }

        public virtual ICollection<UserAssignment> Students { get; set; }

        public virtual ICollection<AssignmentResult> Results { get; set; }
    }
}
