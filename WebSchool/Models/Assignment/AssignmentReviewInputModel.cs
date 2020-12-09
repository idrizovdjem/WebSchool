using System.ComponentModel.DataAnnotations;

namespace WebSchool.Models.Assignment
{
    public class AssignmentReviewInputModel
    {
        [Required]
        public string AssignmentId { get; set; }

        [Required]
        public string StudentId { get; set; }

        [Range(0, 5000)]
        public int Points { get; set; }
    }
}
