using System;
using System.ComponentModel.DataAnnotations;

namespace WebSchool.Data.Models
{
    public class UserAssignment
    {
        [Required]
        public string StudentId { get; set; }

        public virtual ApplicationUser Student { get; set; }

        [Required]
        public string AssignmentId { get; set; }

        public virtual Assignment Assignment { get; set; }

        [Required]
        public string GroupId { get; set; }

        public virtual Group Group { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsSolved { get; set; }
    }
}
