using System;
using System.ComponentModel.DataAnnotations;

namespace WebSchool.Models.Assignment
{
    public class CreateAssignmentInputModel
    {
        [Required]
        [MinLength(5), MaxLength(200)]
        public string AssignmentName { get; set; }

        [Required]
        public string Signature { get; set; }

        [Required]
        public string AssignmentContent { get; set; }

        [Required]
        public DateTime DueDate { get; set; }
    }
}
