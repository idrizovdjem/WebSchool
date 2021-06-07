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

            if(answers == null)
            {
                validationResult.AddErrorMessage("Answers", QuestionConstsants.AnswersLengthMessage);
                return validationResult;
            }

            foreach (var answer in answers)
            {
                ValidateContent(answer.Content, validationResult, answerIndex);

                if (answer.IsCorrect)
                {
                    correctAnswersCount++;
                }

                answerIndex++;
            }

            ValidateCorrectAnswers(correctAnswersCount, validationResult, hasMutlipleAnswers);

            return validationResult;
        }

        private static void ValidateContent(string content, AnswerValidationResult validationResult, int index)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                validationResult.AddErrorMessage($"Answer {index}", AnswerConstants.ContentIsRequiredMessage);
            }
            else
            {
                if (content.Length < AnswerConstants.MinimumContentLength || AnswerConstants.MaximumContentLength < content.Length)
                {
                    validationResult.AddErrorMessage($"Answer {index}", AnswerConstants.ContentLengthMessage);
                }
            }
        }

        private static void ValidateCorrectAnswers(int correctAnswersCount, AnswerValidationResult validationResult, bool hasMutlipleAnswers)
        {
            if (correctAnswersCount == 0)
            {
                validationResult.AddErrorMessage("Overall", AnswerConstants.MissingCorrectAnswerMessage);
            }
            else if (hasMutlipleAnswers == false && correctAnswersCount > 1)
            {
                validationResult.AddErrorMessage("Overall", AnswerConstants.TooMuchCorrectAnswersMessage);
            }
        }
    }
}
