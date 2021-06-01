using WebSchool.ViewModels.Assignment;

namespace WebSchool.Services.Contracts
{
    public interface IAssignmentsService
    {
        UserAssignmentsViewModel GetUserAssignments(string userId);
    }
}
