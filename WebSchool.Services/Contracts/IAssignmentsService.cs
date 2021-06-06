using System.Threading.Tasks;

using WebSchool.ViewModels.Assignment;
using WebSchool.Common.ValidationResults;

namespace WebSchool.Services.Contracts
{
    public interface IAssignmentsService
    {
        CreatedAssignmentViewModel[] GetCreated(string userId);

        Task CreateAsync(string userId, CreateAssignmentInputModel input);

        AssignmentValidationResult ValidateAssignment(CreateAssignmentInputModel input);
    }
}
