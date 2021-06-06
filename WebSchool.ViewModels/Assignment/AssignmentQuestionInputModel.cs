using System.ComponentModel.DataAnnotations;

namespace WebSchool.ViewModels.Assignment
{
    public class AssignmentQuestionInputModel
    {
        [Required]
        [MinLength(1), MaxLength(5000)]
        public string Question { get; set; }

        public bool HasMultipleAnswers { get; set; }

        [Range(1, short.MaxValue)]
        public QuestionAnswerInputModel[] Answers { get; set; }
    }
}
