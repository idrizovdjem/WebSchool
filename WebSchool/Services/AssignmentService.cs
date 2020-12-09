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

        public AssignmentService(ApplicationDbContext context, IClassesService classesService)
        {
            this.context = context;
            this.classesService = classesService;
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
                    Content = string.Empty,
                    Stage = 1
                };

                firstResults.Add(result);
            }

            await this.context.AssignmentResults.AddRangeAsync(firstResults);
            await this.context.SaveChangesAsync();
        }

        public ICollection<AssignmentInformationViewModel> GetTeacherAssignments(string teacherId)
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

        public ICollection<StudentAssignmentInputModel> GetStudentAssignments(string studentId)
        {
            var assignmentIds = this.context.AssignmentResults
                .Where(x => x.StudentId == studentId)
                .Select(x => new StudentAssignmentInputModel()
                {
                    Id = x.AssignmentId,
                    Stage = x.Stage
                })
                .ToList();

            var assignments = new List<StudentAssignmentInputModel>();
            foreach (var assignmentId in assignmentIds)
            {
                var assignment = this.context.Assignments
                    .FirstOrDefault(x => x.Id == assignmentId.Id);

                if (assignment == null)
                {
                    continue;
                }

                var assignmentModel = new StudentAssignmentInputModel()
                {
                    AssignmentName = assignment.AssignmentTitle,
                    DueDate = assignment.DueDate,
                    Id = assignment.Id,
                    Signature = assignment.Signature,
                    Stage = assignmentId.Stage
                };

                assignments.Add(assignmentModel);
            }

            return assignments;
        }

        public SolveAssignmentViewModel GetAssignment(string id)
        {
            return this.context.Assignments
                .Where(x => x.Id == id)
                .Select(x => new SolveAssignmentViewModel()
                {
                    Id = x.Id,
                    AssignmentName = x.AssignmentTitle,
                    AssignmentContent = x.AssignmentContent
                })
                .FirstOrDefault();
        }

        public async Task Solve(string userId, string assignmentId, string answerContent)
        {
            var assignmentResult = this.context.AssignmentResults
                .FirstOrDefault(x => x.AssignmentId == assignmentId && x.StudentId == userId);

            assignmentResult.Content = answerContent;
            assignmentResult.Stage = 2;

            this.context.AssignmentResults.Update(assignmentResult);
            await this.context.SaveChangesAsync();
        }
    }
}
