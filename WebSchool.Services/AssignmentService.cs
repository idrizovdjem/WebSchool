using System;
using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using WebSchool.ViewModels.User;
using System.Collections.Generic;
using WebSchool.Services.Contracts;
using WebSchool.ViewModels.Assignment;

namespace WebSchool.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly ApplicationDbContext context;

        public AssignmentService(ApplicationDbContext context)
        {
            this.context = context;
        }

        //public async Task CreateAssignment(CreateAssignmentInputModel input, string teacherId, string schoolId)
        //{
        //    var assignment = new Assignment()
        //    {
        //        TeacherId = teacherId,
        //        AssignmentTitle = input.AssignmentName,
        //        AssignmentContent = input.AssignmentContent,
        //        DueDate = input.DueDate,
        //        Points = input.Points,
        //        Signature = input.Signature
        //    };

        //    await GenerateResults(input.Signature, schoolId, assignment.Id);

        //    await this.context.Assignments.AddAsync(assignment);
        //    await this.context.SaveChangesAsync();
        //}

        //public async Task GenerateResults(string signature, string schoolId, string assignmentId)
        //{
        //    var students = this.classesService.GetStudentsFromClass(signature, schoolId);
        //    var firstResults = new List<AssignmentResult>();
        //    foreach (var studentId in students)
        //    {
        //        var result = new AssignmentResult()
        //        {
        //            StudentId = studentId,
        //            AssignmentId = assignmentId,
        //            DueDate = DateTime.UtcNow,
        //            Points = 0,
        //            Content = string.Empty,
        //            Stage = 1
        //        };

        //        firstResults.Add(result);
        //    }

        //    await this.context.AssignmentResults.AddRangeAsync(firstResults);
        //    await this.context.SaveChangesAsync();
        //}

        public ICollection<StudentResultViewModel> GetResults(string assignmentId)
        {
            var maxPoints = this.context.Assignments
                .Where(x => x.Id == assignmentId)
                .Select(x => x.Points)
                .FirstOrDefault();

            var students = this.context.AssignmentResults
                .Where(x => x.AssignmentId == assignmentId)
                .Select(x => new StudentResultViewModel()
                {
                    StudentId = x.StudentId,
                    Points = x.Points,
                    MaxPoints = maxPoints,
                    DueDate = x.DueDate.Date,
                    Stage = x.Stage,
                    AssignmentId = x.AssignmentId
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

        public AssignmentSolveResultViewModel GetAssignmentResult(string studentId, string assignmentId)
        {
            var maxPoints = this.context.Assignments
                .Where(x => x.Id == assignmentId)
                .Select(x => x.Points)
                .FirstOrDefault();

            return this.context.AssignmentResults
                .Where(x => x.StudentId == studentId && x.AssignmentId == assignmentId)
                .Select(x => new AssignmentSolveResultViewModel()
                {
                    AssignmentId = x.AssignmentId,
                    StudentId = x.StudentId,
                    AnswerContent = x.Content,
                    MaxPoints = maxPoints
                })
                .FirstOrDefault();
        }

        public async Task Review(AssignmentReviewInputModel input)
        {
            var assignmentResult = this.context.AssignmentResults
                .FirstOrDefault(x => x.AssignmentId == input.AssignmentId && x.StudentId == input.StudentId);

            var assignment = this.context.Assignments
                .FirstOrDefault(x => x.Id == input.AssignmentId);

            if (assignment == null)
            {
                return;
            }

            if (input.Points < 0 || input.Points > assignment.Points)
            {
                return;
            }

            assignmentResult.Points = input.Points;
            assignmentResult.Stage = 3;

            this.context.AssignmentResults.Update(assignmentResult);
            await this.context.SaveChangesAsync();
        }

        public int GetMaxPoints(string id)
        {
            return this.context.Assignments
                .Where(x => x.Id == id)
                .Select(x => x.Points)
                .FirstOrDefault();
        }
    }
}
