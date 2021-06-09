using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using WebSchool.Data;
using WebSchool.Data.Models;
using WebSchool.Services.Common;
using WebSchool.Common.Constants;
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

        public AssignmentResultViewModel[] GetResults(string groupAssignmentId)
        {
            var assignmentId = dbContext.GivenAssignments
                .First(ga => ga.Id == groupAssignmentId).AssignmentId;
            var maxPoints = GetById(assignmentId).AllPoints;

            return dbContext.AssignmentResults
                .Where(ar => ar.GroupAssignmentId == groupAssignmentId)
                .Select(ar => new AssignmentResultViewModel()
                {
                    StudentName = ar.Student.Email,
                    IsSolved = ar.IsSolved,
                    Points = ar.Points,
                    MaxPoints = maxPoints
                })
                .ToArray();
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
