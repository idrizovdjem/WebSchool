using System.ComponentModel.DataAnnotations;

using WebSchool.Common.Constants;

namespace WebSchool.ViewModels.Assignment
{
    public class AssignmentQuestionInputModel
    {
        [Required(ErrorMessage = QuestionConstsants.QuestionIsRequiredMessage)]
        [MinLength(QuestionConstsants.MinimumQuestionLength, ErrorMessage = QuestionConstsants.QuestionLengthMessage)]
        [MaxLength(QuestionConstsants.MaximumQuestionLength, ErrorMessage = QuestionConstsants.QuestionLengthMessage)]
        public string Question { get; set; }

        public bool HasMultipleAnswers { get; set; }

        [Range(QuestionConstsants.MinimumPoints, QuestionConstsants.MaximumPoints, ErrorMessage = QuestionConstsants.InvalidPointsMessage)]
        public byte Points { get; set; }

        public QuestionAnswerInputModel[] Answers { get; set; }
    }
}
