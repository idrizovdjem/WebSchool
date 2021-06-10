using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using WebSchool.Data;
using WebSchool.Data.Models;
using WebSchool.Services.Common;
using WebSchool.Common.Constants;
using WebSchool.ViewModels.Answer;
using WebSchool.ViewModels.Question;
using WebSchool.Common.Enumerations;
using WebSchool.ViewModels.Assignment;
using WebSchool.Common.ValidationResults;

namespace WebSchool.Services.Assignments
{
    public class AssignmentsService : IAssignmentsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IQuestionsService questionsService;
        private readonly IUtilitiesService utilitiesService;

        public AssignmentsService(
            ApplicationDbContext dbContext,
            IQuestionsService questionsService,
            IUtilitiesService utilitiesService)
        {
            this.dbContext = dbContext;
            this.questionsService = questionsService;
            this.utilitiesService = utilitiesService;
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

            var questionsValidationResult = questionsService.ValidateQuestions(input.Questions);
            utilitiesService.MergeErrorMessages(questionsValidationResult, validationResult);

            return validationResult;
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

            var assignmentViewModel = JsonSerializer.Deserialize<AssignmentViewModel>(assignmentString);
            assignmentViewModel.AllPoints = assignmentViewModel.Questions
                .Sum(q => q.Points);
            return assignmentViewModel;
        }

        public async Task GiveAsync(GiveAssignmentInputModel input)
        {
            var groupAssignment = new GivenAssignment()
            {
                AssignmentId = input.AssignmentId,
                GroupId = input.GroupId,
                DueDate = input.DueDate.ToUniversalTime()
            };

            await dbContext.GivenAssignments.AddAsync(groupAssignment);
            await dbContext.SaveChangesAsync();

            await PopulateAssignmentResults(groupAssignment.Id, input.GroupId);
        }

        public GivenAssignmentViewModel[] GetGiven(string userId)
        {
            return dbContext.GivenAssignments
                .Where(ga => ga.Assignment.CreatorId == userId)
                .Select(ga => new GivenAssignmentViewModel()
                {
                    DueDate = ga.DueDate,
                    GroupAssignmentId = ga.Id,
                    GroupName = ga.Group.Name,
                    Title = ga.Assignment.Title,
                    Status = DateTime.UtcNow > ga.DueDate ? GivenAssignmentStatus.Finished : GivenAssignmentStatus.StillGoing
                })
                .ToArray();
        }

        public MyAssignmentViewModel[] GetMyAssignments(string userId)
        {
            var studentRoleId = dbContext.Roles
                .FirstOrDefault(r => r.Name == "Student").Id;

            var userGroupIds = dbContext.UserGroups
                .Where(ug => ug.UserId == userId && ug.RoleId == studentRoleId)
                .Select(ug => ug.GroupId)
                .ToArray();

            return dbContext.GivenAssignments
                .Where(ga => userGroupIds.Contains(ga.GroupId))
                .Select(ga => new MyAssignmentViewModel()
                {
                    DueDate = ga.DueDate,
                    GroupAssignmentId = ga.Id,
                    GroupName = ga.Group.Name,
                    Title = ga.Assignment.Title,
                    Status = DateTime.UtcNow > ga.DueDate ? GivenAssignmentStatus.Finished : GivenAssignmentStatus.StillGoing,
                    IsSolved = dbContext.AssignmentResults
                        .First(ar => ar.StudentId == userId && ar.GroupAssignmentId == ga.Id).IsSolved
                })
                .ToArray();
        }

        public AssignmentResultSummaryViewModel GetResults(string groupAssignmentId)
        {
            var assignmentId = dbContext.GivenAssignments
                .First(ga => ga.Id == groupAssignmentId).AssignmentId;

            var maxPoints = GetById(assignmentId).AllPoints;

            var viewModel = new AssignmentResultSummaryViewModel()
            {
                Title = dbContext.Assignments
                    .Where(a => a.Id == assignmentId)
                    .Select(a => a.Title)
                    .FirstOrDefault(),
                GroupAssignmentId = groupAssignmentId
            };

             viewModel.Results = dbContext.AssignmentResults
                .Where(ar => ar.GroupAssignmentId == groupAssignmentId)
                .Select(ar => new AssignmentResultViewModel()
                {
                    StudentId = ar.StudentId,
                    StudentName = ar.Student.Email,
                    IsSolved = ar.IsSolved,
                    Points = ar.Points,
                    MaxPoints = maxPoints
                })
                .ToArray();

            viewModel.AveragePoints = viewModel.Results.Average(r => r.Points);
            return viewModel;
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
            if (assignmentModel == null)
            {
                solveValidationResult.AddErrorMessage("Assignment", "Invalid assignment");
                return solveValidationResult;
            }

            if (ValidateQuestionsCount(assignmentModel.Questions, input.Questions, solveValidationResult) == false)
            {
                return solveValidationResult;
            }

            ValidateAnswersCount(assignmentModel.Questions, input.Questions, solveValidationResult);
            return solveValidationResult;
        }

        public async Task ReviewSolveAsync(SolveAssignmentInputModel input, string studentId)
        {
            var assignmentModel = GetByGivenId(input.GroupAssignmentId);
            var assignmentResult = dbContext.AssignmentResults
                .FirstOrDefault(ar => ar.StudentId == studentId && ar.GroupAssignmentId == input.GroupAssignmentId);

            assignmentResult.Points = GetPoints(assignmentModel.Questions, input.Questions);
            assignmentResult.IsSolved = true;
            assignmentResult.Content = JsonSerializer.Serialize(input);

            await dbContext.SaveChangesAsync();
        }

        public AssignmentViewModel GetByGivenId(string groupAssignmentId)
        {
            var assignmentId = dbContext.GivenAssignments
                .Where(ga => ga.Id == groupAssignmentId)
                .Select(ga => ga.AssignmentId)
                .FirstOrDefault();

            if (assignmentId == null)
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

        public AssignmentResultPreviewViewModel GetPreview(string groupAssignmentId, string studentId)
        {
            var resultContent = dbContext.AssignmentResults
                .Where(ar => ar.GroupAssignmentId == groupAssignmentId && ar.StudentId == studentId && ar.IsSolved)
                .Select(ar => ar.Content)
                .FirstOrDefault();

            if(string.IsNullOrWhiteSpace(resultContent))
            {
                return null;
            }

            var solvedAssignmentModel = JsonSerializer.Deserialize<SolveAssignmentInputModel>(resultContent);
            var assignmentViewModel = GetByGivenId(groupAssignmentId);
            var previewModel = new AssignmentResultPreviewViewModel()
            {
                Title = assignmentViewModel.Title,
                Questions = new QuestionPreviewViewModel[solvedAssignmentModel.Questions.Length]
            };

            for (var questionIndex = 0; questionIndex < solvedAssignmentModel.Questions.Length; questionIndex++)
            {
                previewModel.Questions[questionIndex] = new QuestionPreviewViewModel()
                {
                    HasMultipleAnswers = assignmentViewModel.Questions[questionIndex].HasMultipleAnswers,
                    Points = assignmentViewModel.Questions[questionIndex].Points,
                    Question = assignmentViewModel.Questions[questionIndex].Question,
                    IsCorrect = solvedAssignmentModel.Questions[questionIndex].IsCorrect,
                    Answers = new AnswerViewModel[solvedAssignmentModel.Questions[questionIndex].Answers.Length]
                };

                for(var answerIndex = 0; answerIndex < solvedAssignmentModel.Questions[questionIndex].Answers.Length; answerIndex++)
                {
                    previewModel.Questions[questionIndex].Answers[answerIndex] = new AnswerViewModel()
                    {
                         IsCorrect = solvedAssignmentModel.Questions[questionIndex].Answers[answerIndex].IsCorrect,
                         Content = assignmentViewModel.Questions[questionIndex].Answers[answerIndex].Content,
                    };
                }
            }

            return previewModel;
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
                    solvedQuestions[questionIndex].IsCorrect = false;
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

                solvedQuestions[questionIndex].IsCorrect = isCorrect;
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

        private async Task PopulateAssignmentResults(string groupAssignmentId, string groupId)
        {
            var studentIds = dbContext.UserGroups
                .Where(ug => ug.GroupId == groupId && ug.Role.Name == "Student")
                .Select(ug => ug.UserId)
                .ToArray();

            foreach (var studentId in studentIds)
            {
                var assignmentResult = new AssignmentResult()
                {
                    GroupAssignmentId = groupAssignmentId,
                    StudentId = studentId,
                    IsSolved = false,
                    Points = 0
                };

                await dbContext.AssignmentResults.AddAsync(assignmentResult);
            }

            await dbContext.SaveChangesAsync();
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
    }
}
