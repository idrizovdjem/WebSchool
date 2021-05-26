using System;
using System.ComponentModel.DataAnnotations;

namespace WebSchool.Data.Models
{
    public class Assignment
    {
        public Assignment()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string TeacherId { get; set; }

        public virtual ApplicationUser Teacher { get; set; }

        [Required]
        [MaxLength(200)]
        public string AssignmentTitle { get; set; }

        [Required]
        public string Signature { get; set; }

        [Required]
        public string AssignmentContent { get; set; }

        public DateTime DueDate { get; set; }

        public int Points { get; set; }
    }
}
