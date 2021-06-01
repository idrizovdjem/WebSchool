using System.Linq;

using WebSchool.Data;
using WebSchool.Services.Contracts;
using WebSchool.Common.Enumerations;
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

        public UserAssignmentsViewModel GetUserAssignments(string userId)
        {
            var viewModel = new UserAssignmentsViewModel();

            viewModel.CreatedAssignments = dbContext.Assignments
                .Where(a => a.TeacherId == userId)
                .Select(a => new AssignmentPreviewViewModel()
                {
                    Id = a.Id,
                    Name = a.Title,
                    GroupName = a.Group.Name,
                    Status = AssignmentStatus.Created
                })
                .ToArray();

            viewModel.NotSolvedAssignments = dbContext.UserAssignments
                .Where(ua => ua.StudentId == userId && ua.IsSolved == false)
                .Select(a => new AssignmentPreviewViewModel()
                {
                    Id = a.Assignment.Id,
                    Name = a.Assignment.Title,
                    GroupName = a.Assignment.Group.Name,
                    Status = AssignmentStatus.NotSolved
                })
                .ToArray();

            viewModel.SolvedAssignments = dbContext.UserAssignments
                .Where(ua => ua.StudentId == userId && ua.IsSolved == true)
                .Select(a => new AssignmentPreviewViewModel()
                {
                    Id = a.Assignment.Id,
                    Name = a.Assignment.Title,
                    GroupName = a.Assignment.Group.Name,
                    Status = AssignmentStatus.NotSolved
                })
                .ToArray();

            return viewModel;
        }
    }
}
