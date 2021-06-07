using WebSchool.Common.Constants;
using WebSchool.ViewModels.Answer;
using WebSchool.Services.Contracts;
using WebSchool.Common.ValidationResults;

namespace WebSchool.Services
{
    public class AnswersService : IAnswersService
    {
        public AnswerValidationResult ValidateAnswers(AnswerInputModel[] answers, bool hasMutlipleAnswers)
        {
            var validationResult = new AnswerValidationResult();
            var correctAnswersCount = 0;
            var answerIndex = 1;

            foreach (var answer in answers)
            {
                if (string.IsNullOrWhiteSpace(answer.Content))
                {
                    validationResult.AddErrorMessage($"Answer {answerIndex}", AnswerConstants.ContentIsRequiredMessage);
                }
                else
                {
                    if (answer.Content.Length < AnswerConstants.MinimumContentLength || AnswerConstants.MaximumContentLength < answer.Content.Length)
                    {
                        validationResult.AddErrorMessage($"Answer {answerIndex}", AnswerConstants.ContentLengthMessage);
                    }
                }

                if (answer.IsCorrect)
                {
                    correctAnswersCount++;
                }

                answerIndex++;
            }

            if (correctAnswersCount == 0)
            {
                validationResult.AddErrorMessage("Overall", AnswerConstants.MissingCorrectAnswerMessage);
            }
            else if (hasMutlipleAnswers == false && correctAnswersCount > 1)
            {
                validationResult.AddErrorMessage("Overall", AnswerConstants.TooMuchCorrectAnswersMessage);
            }

            return validationResult;
        }
    }
}
