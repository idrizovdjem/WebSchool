using WebSchool.Models.User;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebSchool.Models.Assignment;

namespace WebSchool.Services.Contracts
{
    public interface IAssignmentService
    {
        Task CreateAssignment(CreateAssignmentInputModel input, string teacherId, string schoolId);

        ICollection<AssignmentInformationViewModel> GetTeacherAssignments(string teacherId);

        Task GenerateResults(string signature, string schoolId, string assignmentId);

        ICollection<StudentResultViewModel> GetResults(string assignmentId);

        ICollection<StudentAssignmentViewModel> GetStudentAssignments(string studentId);

        SolveAssignmentViewModel GetAssignment(string id);

        Task Solve(string userId, string assignmentId, string answerContent);

        AssignmentResultViewModel GetAssignmentResult(string studentId, string assignmentId);

        Task Review(AssignmentReviewInputModel input);
    }
}
