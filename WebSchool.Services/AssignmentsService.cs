using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using WebSchool.Data;
using WebSchool.Data.Models;
using WebSchool.Common.Constants;
using WebSchool.Services.Contracts;
using WebSchool.ViewModels.Assignment;
using WebSchool.Common.ValidationResults;

namespace WebSchool.Services
{
    public class AssignmentsService : IAssignmentsService
    {
        private readonly ApplicationDbContext dbContext;

        public AssignmentsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(string userId, CreateAssignmentInputModel input)
        {
            var assignmentString = JsonSerializer.Serialize(input);
            var assignment = new Assignment()
            {
                CreatorId = userId,
                Content = assignmentString,
                Title = input.Title
            };

            await dbContext.Assignments.AddAsync(assignment);
            await dbContext.SaveChangesAsync();
        }

        public CreatedAssignmentViewModel[] GetCreated(string userId)
        {
            return dbContext.Assignments
                .Where(a => a.CreatorId == userId)
                .Select(a => new CreatedAssignmentViewModel()
                {
                    Id = a.Id,
                    Title = a.Title
                })
                .ToArray();
        }

        public AssignmentValidationResult ValidateAssignment(CreateAssignmentInputModel input)
        {
            var validationResult = new AssignmentValidationResult();
            ValidateTitle(input.Title, validationResult);

            var questionsValidationResult = ValidateQuestions(input.Questions);
            MergeErrorMessages(questionsValidationResult, validationResult);

            return validationResult;
        }

        private static void ValidateTitle(string title, AssignmentValidationResult validationResult)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                validationResult.AddErrorMessage("Title", AssignmentConstants.TitleIsRequiredMessage);
            }
            else
            {
                if (title.Length < AssignmentConstants.MinimumTitleLength || AssignmentConstants.MaximumTitleLength < title.Length)
                {
                    validationResult.AddErrorMessage("Title", AssignmentConstants.TitleLengthMessage);
                }
            }
        }

        private static QuestionValidationResult ValidateQuestions(AssignmentQuestionInputModel[] questions)
        {
            var validationResult = new QuestionValidationResult();

            var questionIndex = 1;
            foreach(var question in questions)
            {
                if(string.IsNullOrWhiteSpace(question.Question))
                {
                    validationResult.AddErrorMessage($"Question {questionIndex}", QuestionConstsants.QuestionIsRequiredMessage);
                }
                else
                {
                    if(question.Question.Length < QuestionConstsants.MinimumQuestionLength || QuestionConstsants.MaximumQuestionLength < question.Question.Length)
                    {
                        validationResult.AddErrorMessage($"Question {questionIndex}", QuestionConstsants.QuestionLengthMessage);
                    }
                }

                if(question.Points < QuestionConstsants.MinimumPoints || QuestionConstsants.MaximumPoints < question.Points)
                {
                    validationResult.AddErrorMessage($"Question {questionIndex}", QuestionConstsants.InvalidPointsMessage);
                }

                if(question.Answers.Length < QuestionConstsants.MinimumAnswersCount || QuestionConstsants.MaximumAnswersCount < question.Answers.Length)
                {
                    validationResult.AddErrorMessage($"Question {questionIndex}", QuestionConstsants.AnswersLengthMessage);
                }
                else
                {
                    var answersValidationResult = ValidateAnswers(question.Answers, question.HasMultipleAnswers);
                    MergeErrorMessages(answersValidationResult, validationResult);
                }


                questionIndex++;
            }

            return validationResult;
        }

        private static AnswerValidationResult ValidateAnswers(QuestionAnswerInputModel[] answers, bool hasMutlipleAnswers)
        {
            var validationResult = new AnswerValidationResult();
            var correctAnswersCount = 0;
            var answerIndex = 1;

            foreach(var answer in answers)
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

                if(answer.IsCorrect)
                {
                    correctAnswersCount++;
                }

                answerIndex++;
            }

            if(correctAnswersCount == 0)
            {
                validationResult.AddErrorMessage("Overall", AnswerConstants.MissingCorrectAnswerMessage);
            }
            else if(hasMutlipleAnswers == false && correctAnswersCount > 1)
            {
                validationResult.AddErrorMessage("Overall", AnswerConstants.TooMuchCorrectAnswersMessage);
            }

            return validationResult;
        }

        private static void MergeErrorMessages(BaseValidationResult source, BaseValidationResult destination)
        {
            foreach (var key in source.Errors.Keys)
            {
                foreach (var message in source.Errors[key])
                {
                    destination.AddErrorMessage(key, message);
                }
            }
        }

        public AssignmentViewModel GetById(string id)
        {
            var assignmentString = dbContext.Assignments
                .FirstOrDefault(a => a.Id == id)
                .Content;

            if(assignmentString == null)
            {
                return null;
            }

            return JsonSerializer.Deserialize<AssignmentViewModel>(assignmentString);
        }
    }
}
