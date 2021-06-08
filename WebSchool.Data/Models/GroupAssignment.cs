using System;
using System.ComponentModel.DataAnnotations;

namespace WebSchool.Data.Models
{
    public class GroupAssignment
    {
        public GroupAssignment()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string AssignmentId { get; set; }

        public virtual Assignment Assignment { get; set; }

        [Required]
        public string GroupId { get; set; }

        public virtual Group Group { get; set; }

        public DateTime DueDate { get; set; }
    }
}
