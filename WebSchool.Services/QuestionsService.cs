using WebSchool.Common.Constants;
using WebSchool.ViewModels.Answer;
using WebSchool.Services.Contracts;
using WebSchool.ViewModels.Question;
using WebSchool.Common.ValidationResults;

namespace WebSchool.Services
{
    public class QuestionsService : IQuestionsService
    {
        private readonly IAnswersService answersService;
        private readonly IUtilitiesService utilitiesService;

        public QuestionsService(
            IAnswersService answersService,
            IUtilitiesService utilitiesService)
        {
            this.answersService = answersService;
            this.utilitiesService = utilitiesService;
        }

        public QuestionValidationResult ValidateQuestions(QuestionInputModel[] questions)
        {
            var validationResult = new QuestionValidationResult();

            if(questions == null)
            {
                validationResult.AddErrorMessage("Questions", AssignmentConstants.InvalidQuestionsCountMessage);
                return validationResult;
            }

            var questionIndex = 1;
            foreach (var question in questions)
            {
                ValidateQuestionTitle(question.Question, validationResult, questionIndex);
                ValidatePoints(question.Points, validationResult, questionIndex);
                ValidateAnswers(question.Answers, validationResult, questionIndex, question.HasMultipleAnswers);

                questionIndex++;
            }

            return validationResult;
        }

        private static void ValidateQuestionTitle(string question, QuestionValidationResult validationResult, int index)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                validationResult.AddErrorMessage($"Question {index}", QuestionConstsants.QuestionIsRequiredMessage);
            }
            else
            {
                if (question.Length < QuestionConstsants.MinimumQuestionLength || QuestionConstsants.MaximumQuestionLength < question.Length)
                {
                    validationResult.AddErrorMessage($"Question {index}", QuestionConstsants.QuestionLengthMessage);
                }
            }
        }

        private static void ValidatePoints(byte points, QuestionValidationResult validationResult, int index)
        {
            if (points < QuestionConstsants.MinimumPoints || QuestionConstsants.MaximumPoints < points)
            {
                validationResult.AddErrorMessage($"Question {index}", QuestionConstsants.InvalidPointsMessage);
            }
        }

        private void ValidateAnswers(AnswerInputModel[] answers, QuestionValidationResult validationResult, int index, bool hasMultipleAnswers)
        {
            if(answers == null)
            {
                validationResult.AddErrorMessage($"Question {index}", QuestionConstsants.AnswersLengthMessage);
                return;
            }

            if (answers.Length < QuestionConstsants.MinimumAnswersCount || QuestionConstsants.MaximumAnswersCount < answers.Length)
            {
                validationResult.AddErrorMessage($"Question {index}", QuestionConstsants.AnswersLengthMessage);
            }
            else
            {
                var answersValidationResult = answersService.ValidateAnswers(answers, hasMultipleAnswers);
                utilitiesService.MergeErrorMessages(answersValidationResult, validationResult);
            }
        }
    }
}
