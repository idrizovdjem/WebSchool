using System.Linq;

using WebSchool.Data;
using WebSchool.Services.Contracts;
using WebSchool.ViewModels.Assignment;

namespace WebSchool.Services
{
    public class AssignmentsService : IAssignmentsService
    {
        private readonly ApplicationDbContext dbContext;

        public AssignmentsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public CreatedAssignmentViewModel[] GetCreated(string userId)
        {
            return dbContext.Assignments
                .Where(a => a.CreatorId == userId)
                .Select(a => new CreatedAssignmentViewModel()
                {
                    Id = a.Id,
                    Title = a.Title
                })
                .ToArray();
        }
    }
}
