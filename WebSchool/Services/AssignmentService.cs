using System;
using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using WebSchool.Models.User;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebSchool.Models.Assignment;
using WebSchool.Services.Contracts;

namespace WebSchool.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly ApplicationDbContext context;
        private readonly IClassesService classesService;
        private readonly IUsersService usersService;

        public AssignmentService(ApplicationDbContext context, IClassesService classesService, IUsersService usersService)
        {
            this.context = context;
            this.classesService = classesService;
            this.usersService = usersService;
        }

        public async Task CreateAssignment(CreateAssignmentInputModel input, string teacherId, string schoolId)
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

            await GenerateResults(input.Signature, schoolId, assignment.Id);

            await this.context.Assignments.AddAsync(assignment);
            await this.context.SaveChangesAsync();
        }

        public async Task GenerateResults(string signature, string schoolId, string assignmentId)
        {
            var students = this.classesService.GetStudentsFromClass(signature, schoolId);
            var firstResults = new List<AssignmentResult>();
            foreach (var studentId in students)
            {
                var result = new AssignmentResult()
                {
                    StudentId = studentId,
                    AssignmentId = assignmentId,
                    DueDate = DateTime.UtcNow,
                    Points = 0,
                    Stage = 1
                };

                firstResults.Add(result);
            }

            await this.context.AssignmentResults.AddRangeAsync(firstResults);
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

        public ICollection<StudentResultViewModel> GetResults(string assignmentId)
        {
            var students = this.context.AssignmentResults
                .Where(x => x.AssignmentId == assignmentId)
                .Select(x => new StudentResultViewModel()
                {
                    StudentId = x.StudentId,
                    Points = x.Points,
                    DueDate = x.DueDate,
                    Stage = x.Stage
                })
                .ToList();

            foreach (var student in students)
            {
                var user = this.context.Users
                    .FirstOrDefault(x => x.Id == student.StudentId);
                student.FirstName = user.FirstName;
                student.LastName = user.LastName;
            }

            return students;
        }
    }
}
