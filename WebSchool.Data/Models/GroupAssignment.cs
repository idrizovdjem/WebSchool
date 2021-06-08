using System;
using System.ComponentModel.DataAnnotations;

namespace WebSchool.Data.Models
{
    public class GroupAssignment
    {

        [Required]
        public string AssignmentId { get; set; }

        public virtual Assignment Assignment { get; set; }

        [Required]
        public string GroupId { get; set; }

        public virtual Group Group { get; set; }

        public DateTime DueDate { get; set; }
    }
}
