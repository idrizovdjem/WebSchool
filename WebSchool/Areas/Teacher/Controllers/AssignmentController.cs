using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSchool.Models.Assignment;
using WebSchool.Services.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace WebSchool.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class AssignmentController : Controller
    {
        private readonly IAssignmentService assignmentService;
        private readonly IUsersService usersService;

        public AssignmentController(IAssignmentService assignmentService, IUsersService usersService)
        {
            this.assignmentService = assignmentService;
            this.usersService = usersService;
        }

        public IActionResult GiveAssignment()
        {
            return View();
        }

        [HttpPost]
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

            var user = await this.usersService.GetUser(this.User);
            await this.assignmentService.CreateTask(input, user.Id);

            return RedirectToAction("Results");
        }

        public IActionResult Results()
        {
            return View();
        }
    }
}
