using WebSchool.ViewModels.Question;
using WebSchool.Common.ValidationResults;

namespace WebSchool.Services.Assignments
{
    public interface IQuestionsService
    {
        QuestionValidationResult ValidateQuestions(QuestionInputModel[] questions);
    }
}
