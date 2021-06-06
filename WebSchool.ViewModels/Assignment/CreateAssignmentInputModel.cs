using System.ComponentModel.DataAnnotations;

namespace WebSchool.ViewModels.Assignment
{
    public class CreateAssignmentInputModel
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Range(1, short.MaxValue)]
        public AssignmentQuestionInputModel[] Questions { get; set; }
    }
}
