using System.Linq;
using WebSchool.Data;
using System.Collections.Generic;
using WebSchool.Services.Contracts;
using WebSchool.ViewModels.Assignment;

namespace WebSchool.Services
{
    public class StudentsService : IStudentsService
    {
        private readonly ApplicationDbContext context;
        private readonly IRolesService rolesService;

        public StudentsService(ApplicationDbContext context, IRolesService rolesService)
        {
            this.context = context;
            this.rolesService = rolesService;
        }

        public List<string> GetStudentIdsWithMatchingEmail(string email, string signature, string schoolId)
        {
            var users = this.context.Users
                .Where(x => x.SchoolId == schoolId && x.Email.Contains(email))
                .ToList();

            var schoolClass = this.context.SchoolClasses
                .FirstOrDefault(x => x.SchoolId == schoolId && x.Signature == x.Signature);

            var filteredUsers = new List<string>();
            foreach (var user in users)
            {
                if (this.context.UserClasses.Any(x => x.UserId == user.Id))
                {
                    continue;
                }

                if (this.rolesService.GetUserRole(user.Id) != "Student")
                {
                    continue;
                }

                filteredUsers.Add(user.Email);
            }

            return filteredUsers;
        }

        public ICollection<StudentAssignmentViewModel> GetStudentAssignments(string studentId)
        {
            var assignmentIds = this.context.AssignmentResults
                .Where(x => x.StudentId == studentId && x.Stage == 1)
                .Select(x => new StudentAssignmentViewModel()
                {
                    Id = x.AssignmentId,
                    Stage = x.Stage,
                    Points = x.Points
                })
                .ToList();

            var assignments = new List<StudentAssignmentViewModel>();
            foreach (var assignmentId in assignmentIds)
            {
                var assignment = this.context.Assignments
                    .FirstOrDefault(x => x.Id == assignmentId.Id);

                if (assignment == null)
                {
                    continue;
                }

                var assignmentModel = new StudentAssignmentViewModel()
                {
                    AssignmentName = assignment.AssignmentTitle,
                    DueDate = assignment.DueDate,
                    Id = assignment.Id,
                    Signature = assignment.Signature,
                    Stage = assignmentId.Stage,
                    Points = assignmentId.Points
                };

                assignments.Add(assignmentModel);
            }

            return assignments;
        }

        public ICollection<StudentAssignmentViewModel> GetStudentResults(string studentId)
        {
            var assignmentIds = this.context.AssignmentResults
                .Where(x => x.StudentId == studentId && x.Stage != 1)
                .Select(x => new StudentAssignmentViewModel()
                {
                    Id = x.AssignmentId,
                    Stage = x.Stage,
                    Points = x.Points
                })
                .ToList();

            var assignments = new List<StudentAssignmentViewModel>();
            foreach (var assignmentId in assignmentIds)
            {
                var assignment = this.context.Assignments
                    .FirstOrDefault(x => x.Id == assignmentId.Id);

                if (assignment == null)
                {
                    continue;
                }

                var assignmentModel = new StudentAssignmentViewModel()
                {
                    AssignmentName = assignment.AssignmentTitle,
                    DueDate = assignment.DueDate,
                    Id = assignment.Id,
                    Signature = assignment.Signature,
                    Stage = assignmentId.Stage,
                    Points = assignmentId.Points
                };

                assignments.Add(assignmentModel);
            }

            return assignments;
        }
    }
}
