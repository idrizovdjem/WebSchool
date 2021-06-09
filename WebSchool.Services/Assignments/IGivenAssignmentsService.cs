using System.Threading.Tasks;

using WebSchool.ViewModels.Assignment;
using WebSchool.Common.ValidationResults;

namespace WebSchool.Services.Assignments
{
    public interface IGivenAssignmentsService
    {
        AssignmentViewModel GetForSolve(string groupAssignmentId);

        SolveValidationResult ValidateSolve(SolveAssignmentInputModel input);

        Task ReviewSolveAsync(SolveAssignmentInputModel input, string studentId);

        AssignmentViewModel GetByGivenId(string groupAssignmentId);
    }
}
