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

            return JsonSerializer.Deserialize<AssignmentViewModel>(assignmentString);
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
