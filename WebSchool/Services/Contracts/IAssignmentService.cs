using System.Threading.Tasks;
using WebSchool.Models.Assignment;

namespace WebSchool.Services.Contracts
{
    public interface IAssignmentService
    {
        Task CreateTask(CreateAssignmentInputModel input, string teacherId);
    }
}
