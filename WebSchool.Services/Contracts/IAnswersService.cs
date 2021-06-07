using WebSchool.ViewModels.Answer;
using WebSchool.Common.ValidationResults;

namespace WebSchool.Services.Contracts
{
    public interface IAnswersService
    {
        AnswerValidationResult ValidateAnswers(AnswerInputModel[] answers, bool hasMutlipleAnswers);
    }
}
