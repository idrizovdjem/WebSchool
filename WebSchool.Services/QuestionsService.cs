using WebSchool.Common.Constants;
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

            var questionIndex = 1;
            foreach (var question in questions)
            {
                if (string.IsNullOrWhiteSpace(question.Question))
                {
                    validationResult.AddErrorMessage($"Question {questionIndex}", QuestionConstsants.QuestionIsRequiredMessage);
                }
                else
                {
                    if (question.Question.Length < QuestionConstsants.MinimumQuestionLength || QuestionConstsants.MaximumQuestionLength < question.Question.Length)
                    {
                        validationResult.AddErrorMessage($"Question {questionIndex}", QuestionConstsants.QuestionLengthMessage);
                    }
                }

                if (question.Points < QuestionConstsants.MinimumPoints || QuestionConstsants.MaximumPoints < question.Points)
                {
                    validationResult.AddErrorMessage($"Question {questionIndex}", QuestionConstsants.InvalidPointsMessage);
                }

                if (question.Answers.Length < QuestionConstsants.MinimumAnswersCount || QuestionConstsants.MaximumAnswersCount < question.Answers.Length)
                {
                    validationResult.AddErrorMessage($"Question {questionIndex}", QuestionConstsants.AnswersLengthMessage);
                }
                else
                {
                    var answersValidationResult = answersService.ValidateAnswers(question.Answers, question.HasMultipleAnswers);
                    utilitiesService.MergeErrorMessages(answersValidationResult, validationResult);
                }

                questionIndex++;
            }

            return validationResult;
        }
    }
}
