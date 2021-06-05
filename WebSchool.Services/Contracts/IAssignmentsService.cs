using WebSchool.ViewModels.Assignment;

namespace WebSchool.Services.Contracts
{
    public interface IAssignmentsService
    {
        CreatedAssignmentViewModel[] GetCreated(string userId);
    }
}
