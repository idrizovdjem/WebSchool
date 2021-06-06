using System.ComponentModel.DataAnnotations;

using WebSchool.Common.Constants;

namespace WebSchool.ViewModels.Assignment
{
    public class QuestionAnswerInputModel
    {
        [Required(ErrorMessage = AnswerConstants.ContentIsRequiredMessage)]
        [MinLength(AnswerConstants.MinimumContentLength, ErrorMessage = AnswerConstants.ContentLengthMessage)]
        [MaxLength(AnswerConstants.MaximumContentLength, ErrorMessage = AnswerConstants.ContentLengthMessage)]
        public string Content { get; set; }

        public bool IsCorrect { get; set; }
    }
}
