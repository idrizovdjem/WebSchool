using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using WebSchool.Data;
using WebSchool.ViewModels.Question;
using WebSchool.ViewModels.Assignment;
using WebSchool.Common.ValidationResults;

namespace WebSchool.Services.Assignments
{
    public class GivenAssignmentsService : IGivenAssignmentsService
    {
        private readonly ApplicationDbContext dbContext;

        public GivenAssignmentsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public AssignmentViewModel GetForSolve(string groupAssignmentId)
        {
            var assignmentViewModel = GetByGivenId(groupAssignmentId);
            foreach (var question in assignmentViewModel.Questions)
            {
                foreach (var answer in question.Answers)
                {
                    answer.IsCorrect = false;
                }
            }

            return assignmentViewModel;
        }

        public SolveValidationResult ValidateSolve(SolveAssignmentInputModel input)
        {
            var solveValidationResult = new SolveValidationResult();
            var assignmentModel = GetByGivenId(input.GroupAssignmentId);
            if(assignmentModel == null)
            {
                solveValidationResult.AddErrorMessage("Assignment", "Invalid assignment");
                return solveValidationResult;
            }

            if (ValidateQuestionsCount(assignmentModel.Questions, input.Questions, solveValidationResult) == false)
            {
                return solveValidationResult;
            }

            ValidateQuestionsCount(assignmentModel.Questions, input.Questions, solveValidationResult);
            return solveValidationResult;
        }

        public async Task ReviewSolveAsync(SolveAssignmentInputModel input, string studentId)
        {
            var assignmentModel = GetByGivenId(input.GroupAssignmentId);
            var assignmentResult = dbContext.AssignmentResults
                .FirstOrDefault(ar => ar.StudentId == studentId && ar.GroupAssignmentId == input.GroupAssignmentId);

            assignmentResult.Points = GetPoints(assignmentModel.Questions, input.Questions);
            assignmentResult.IsSolved = true;

            await dbContext.SaveChangesAsync();
        }

        public AssignmentViewModel GetByGivenId(string groupAssignmentId)
        {
            var assignmentId = dbContext.GivenAssignments
                .Where(ga => ga.Id == groupAssignmentId)
                .Select(ga => ga.AssignmentId)
                .FirstOrDefault();

            if(assignmentId == null)
            {
                return null;
            }

            var assignmentContent = dbContext.Assignments
                .FirstOrDefault(a => a.Id == assignmentId)?.Content;
            if (string.IsNullOrWhiteSpace(assignmentContent))
            {
                return null;
            }

            return JsonSerializer.Deserialize<AssignmentViewModel>(assignmentContent);
        }

        private static int GetPoints(QuestionViewModel[] originalQuestions, SolveQuestionInputModel[] solvedQuestions)
        {
            var points = 0;
            for (var questionIndex = 0; questionIndex < solvedQuestions.Length; questionIndex++)
            {
                var originalCorrectAnswers = originalQuestions[questionIndex].Answers
                    .Count(a => a.IsCorrect);
                var currentCorrectAnswers = solvedQuestions[questionIndex].Answers
                    .Count(a => a.IsCorrect);

                if (originalCorrectAnswers != currentCorrectAnswers)
                {
                    continue;
                }

                var isCorrect = true;
                for (var answerIndex = 0; answerIndex < solvedQuestions[questionIndex].Answers.Length; answerIndex++)
                {
                    if (solvedQuestions[questionIndex].Answers[answerIndex].IsCorrect != originalQuestions[questionIndex].Answers[answerIndex].IsCorrect)
                    {
                        isCorrect = false;
                        break;
                    }
                }

                if (isCorrect)
                {
                    points += originalQuestions[questionIndex].Points;
                }
            }

            return points;
        }

        private static bool ValidateQuestionsCount(QuestionViewModel[] originalQuestions, SolveQuestionInputModel[] solvedQuestions, SolveValidationResult validationResult)
        {
            if (originalQuestions.Length != solvedQuestions.Length)
            {
                validationResult.AddErrorMessage("Questions", "Questions count is invalid");
                return false;
            }

            return true;
        }

        private static bool ValidateAnswersCount(QuestionViewModel[] originalQuestions, SolveQuestionInputModel[] solvedQuestions, SolveValidationResult validationResult)
        {
            for (var i = 0; i < solvedQuestions.Length; i++)
            {
                if (solvedQuestions[i].Answers.Length != originalQuestions[i].Answers.Length)
                {
                    validationResult.AddErrorMessage("Answers", "Answers count is invalid");
                    return false;
                }
            }

            return true;
        }
    }
}
