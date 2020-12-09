using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using WebSchool.Models.Assignment;
using WebSchool.Services.Contracts;
using System.Collections.Generic;

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

        public ICollection<AssignmentInformationViewModel> GetAssignments(string teacherId)
        {
            return this.context.Assignments
                .Where(x => x.TeacherId == teacherId)
                .Select(x => new AssignmentInformationViewModel()
                {
                    Id = x.Id,
                    AssignmentName = x.AssignmentTitle,
                    Signature = x.Signature,
                    DueDate = x.DueDate
                })
                .OrderByDescending(x => x.DueDate)
                .ToList();
        }
    }
}
