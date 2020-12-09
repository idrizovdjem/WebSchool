using System;
using System.ComponentModel.DataAnnotations;

namespace WebSchool.Data.Models
{
    public class AssignmentResult
    {
        public AssignmentResult()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string AssignmentId { get; set; }

        [Required]
        public string StudentId { get; set; }

        public int Points { get; set; }

        public DateTime DueDate { get; set; }
    }
}
