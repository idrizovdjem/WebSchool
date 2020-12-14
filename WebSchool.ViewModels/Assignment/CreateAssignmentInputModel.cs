using System;
using System.ComponentModel.DataAnnotations;

namespace WebSchool.ViewModels.Assignment
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

        [Range(0, 500)]
        public int Points { get; set; }
    }
}
