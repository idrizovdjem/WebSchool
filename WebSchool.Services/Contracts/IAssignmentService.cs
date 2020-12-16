using WebSchool.ViewModels.User;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebSchool.ViewModels.Assignment;

namespace WebSchool.Services.Contracts
{
    public interface IAssignmentService
    {
        Task CreateAssignment(CreateAssignmentInputModel input, string teacherId, string schoolId);

        ICollection<AssignmentInformationViewModel> GetTeacherAssignments(string teacherId);

        Task GenerateResults(string signature, string schoolId, string assignmentId);

        ICollection<StudentResultViewModel> GetResults(string assignmentId);

        SolveAssignmentViewModel GetAssignment(string id);

        Task Solve(string userId, string assignmentId, string answerContent);

        AssignmentSolveResultViewModel GetAssignmentResult(string studentId, string assignmentId);

        Task Review(AssignmentReviewInputModel input);
    }
}
