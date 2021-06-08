using System;
using System.ComponentModel.DataAnnotations;

namespace WebSchool.ViewModels.Assignment
{
    public class GiveAssignmentInputModel
    {
        [Required]
        public string GroupId { get; set; }

        [Required]
        public string AssignmentId { get; set; }

        public DateTime DueDate { get; set; }
    }
}
