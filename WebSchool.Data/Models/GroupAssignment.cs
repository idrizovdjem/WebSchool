using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebSchool.Data.Models
{
    public class GroupAssignment
    {
        public GroupAssignment()
        {
            Id = Guid.NewGuid().ToString();
            Results = new HashSet<AssignmentResult>();
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

        public virtual ICollection<AssignmentResult> Results { get; set; }
    }
}
