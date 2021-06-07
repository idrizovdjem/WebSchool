using WebSchool.ViewModels.Question;
using WebSchool.Common.ValidationResults;

namespace WebSchool.Services.Contracts
{
    public interface IQuestionsService
    {
        QuestionValidationResult ValidateQuestions(QuestionInputModel[] questions);
    }
}
