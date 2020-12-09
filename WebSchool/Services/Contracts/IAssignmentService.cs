using WebSchool.Models.User;
using System.Threading.Tasks;
using WebSchool.Models.Assignment;
using System.Collections.Generic;

namespace WebSchool.Services.Contracts
{
    public interface IAssignmentService
    {
        Task CreateAssignment(CreateAssignmentInputModel input, string teacherId, string schoolId);

        ICollection<AssignmentInformationViewModel> GetAssignments(string teacherId);

        Task GenerateResults(string signature, string schoolId, string assignmentId);

        ICollection<StudentResultViewModel> GetResults(string assignmentId);
    }
}
