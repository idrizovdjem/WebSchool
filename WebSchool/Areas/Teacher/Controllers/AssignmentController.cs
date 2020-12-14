using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSchool.ViewModels.Assignment;
using WebSchool.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebSchool.Data.Models;

namespace WebSchool.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class AssignmentController : Controller
    {
        private readonly IUsersService usersService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAssignmentService assignmentService;

        public AssignmentController(IAssignmentService assignmentService, IUsersService usersService, UserManager<ApplicationUser> userManager)
        {
            this.usersService = usersService;
            this.userManager = userManager;
            this.assignmentService = assignmentService;
        }

        public IActionResult GiveAssignment()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GiveAssignment(CreateAssignmentInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return View(input);
            }

            if (input.DueDate < DateTime.Now)
            {
                this.ModelState.AddModelError("Datetime", "Due date cannot be yearlier than today!");
                return View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            await this.assignmentService.CreateAssignment(input, user.Id, user.SchoolId);

            return RedirectToAction("Results");
        }

        public IActionResult Results()
        {
            return View();
        }

        public async Task<IActionResult> GetAssignments()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var assignments = this.assignmentService.GetTeacherAssignments(user.Id);

            return Json(assignments);
        }

        public IActionResult AssignmentInformation(string assignmentId)
        {
            var studentResults = this.assignmentService.GetResults(assignmentId);

            return View(studentResults);
        }

        public IActionResult Review(string studentId, string assignmentId)
        {
            var assignmentResult = this.assignmentService.GetAssignmentResult(studentId, assignmentId);
            if (assignmentResult == null)
            {
                return RedirectToAction("Results");
            }

            return View(assignmentResult);
        }

        [HttpPost]
        public async Task<IActionResult> Review(AssignmentReviewInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return RedirectToAction("Results");
            }

            await this.assignmentService.Review(input);

            return RedirectToAction("Results");
        }
    }
}
