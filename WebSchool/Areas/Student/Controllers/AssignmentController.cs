using WebSchool.Data.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSchool.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace WebSchool.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    public class AssignmentController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ISchoolService schoolService;
        private readonly IAssignmentService assignmentService;
        private readonly IStudentsService studentsService;

        public AssignmentController(
            UserManager<ApplicationUser> userManager,
            ISchoolService schoolService,
            IAssignmentService assignmentService,
            IStudentsService studentsService)
        {
            this.userManager = userManager;
            this.schoolService = schoolService;
            this.assignmentService = assignmentService;
            this.studentsService = studentsService;
        }

        public async Task<IActionResult> Assignments()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var assignments = this.studentsService.GetStudentAssignments(user.Id);

            return View(assignments);
        }

        public IActionResult Solve(string Id)
        {
            var assignment = this.assignmentService.GetAssignment(Id);
            if (assignment == null)
            {
                return RedirectToAction("Assignments");
            }

            return View(assignment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Solve(string assignmentId, string answerContent)
        {
            if (string.IsNullOrWhiteSpace(assignmentId))
            {
                return RedirectToAction("Assignments");
            }

            if (string.IsNullOrWhiteSpace(answerContent))
            {
                return RedirectToAction("Assignments");
            }

            var user = await this.userManager.GetUserAsync(this.User);
            await this.assignmentService.Solve(user.Id, assignmentId, answerContent);

            return RedirectToAction("Assignments");
        }

        public async Task<IActionResult> Results()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var results = this.studentsService.GetStudentResults(user.Id);
            return View(results);
        }
    }
}
