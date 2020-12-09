using WebSchool.Data;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using WebSchool.Models.Assignment;
using WebSchool.Services.Contracts;

namespace WebSchool.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly ApplicationDbContext context;

        public AssignmentService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task CreateTask(CreateAssignmentInputModel input, string teacherId)
        {
            var assignment = new Assignment()
            {
                TeacherId = teacherId,
                AssignmentTitle = input.AssignmentName,
                AssignmentContent = input.AssignmentContent,
                DueDate = input.DueDate,
                Points = input.Points,
                Signature = input.Signature
            };

            await this.context.Assignments.AddAsync(assignment);
            await this.context.SaveChangesAsync();
        }
    }
}
