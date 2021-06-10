using System.Threading.Tasks;

using WebSchool.ViewModels.Assignment;
using WebSchool.Common.ValidationResults;

namespace WebSchool.Services.Assignments
{
    public interface IAssignmentsService
    {
        CreatedAssignmentViewModel[] GetCreated(string userId);

        Task CreateAsync(string userId, CreateAssignmentInputModel input);

        AssignmentValidationResult ValidateAssignment(CreateAssignmentInputModel input);

        AssignmentViewModel GetById(string id);

        Task GiveAsync(GiveAssignmentInputModel input);

        GivenAssignmentViewModel[] GetGiven(string userId);

        MyAssignmentViewModel[] GetMyAssignments(string userId);

        AssignmentResultSummaryViewModel GetResults(string groupAssignmentId);

        AssignmentViewModel GetForSolve(string groupAssignmentId);

        SolveValidationResult ValidateSolve(SolveAssignmentInputModel input);

        Task ReviewSolveAsync(SolveAssignmentInputModel input, string studentId);

        AssignmentResultPreviewViewModel GetPreview(string groupAssignmentId, string studentId);

        AssignmentViewModel GetByGivenId(string groupAssignmentId);

        CreateAssignmentInputModel GetForEdit(string id);

        Task EditAsync(CreateAssignmentInputModel input, string id);
    }
}
