using System.Threading.Tasks;
using System.Collections.Generic;
using WebSchool.Models.Assignment;

namespace WebSchool.Services.Contracts
{
    public interface IAssignmentService
    {
        Task CreateTask(CreateAssignmentInputModel input, string teacherId);

        ICollection<AssignmentInformationViewModel> GetAssignments(string teacherId);
    }
}
