using System.ComponentModel.DataAnnotations;

namespace WebSchool.ViewModels.Assignment
{
    public class QuestionAnswerInputModel
    {
        [Required]
        [MinLength(1), MaxLength(500)]
        public string Content { get; set; }

        public bool IsCorrect { get; set; }
    }
}
