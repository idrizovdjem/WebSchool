using System.Collections.Generic;
using WebSchool.ViewModels.Assignment;

namespace WebSchool.Services.Contracts
{
    public interface IStudentsService
    {
        // List<string> GetStudentIdsWithMatchingEmail(string email, string signature, string schoolId);

        public ICollection<StudentAssignmentViewModel> GetStudentAssignments(string studentId);

        public ICollection<StudentAssignmentViewModel> GetStudentResults(string studentId);
    }
}
