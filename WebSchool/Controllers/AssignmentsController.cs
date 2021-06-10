using System;
using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.Services.Common;
using WebSchool.Common.Enumerations;
using WebSchool.Services.Assignments;
using WebSchool.ViewModels.Assignment;

namespace WebSchool.WebApplication.Controllers
{
    [Authorize]
    public class AssignmentsController : Controller
    {
        private readonly IAssignmentsService assignmentsService;
        private readonly IUsersService usersService;

        public AssignmentsController(
            IAssignmentsService assignmentsService,
            IUsersService usersService)
        {
            this.assignmentsService = assignmentsService;
            this.usersService = usersService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAssignmentInputModel input)
        {
            if(ModelState.IsValid == false)
            {
                return View(input);
            }

            var validationResult = assignmentsService.ValidateAssignment(input);
            if(validationResult.IsValid == false)
            {
                foreach(var key in validationResult.Errors.Keys)
                {
                    foreach(var message in validationResult.Errors[key])
                    {
                        ModelState.AddModelError(key, message);
                    }
                }

                return View(input);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await assignmentsService.CreateAsync(userId, input);

            return RedirectToAction(nameof(Created));
        }

        public IActionResult Created()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var createdAssignments = assignmentsService.GetCreated(userId);
            return View(createdAssignments);
        }

        public IActionResult Details(string assignmentId)
        {
            var assigmentViewModel = assignmentsService.GetById(assignmentId);
            if(assigmentViewModel == null)
            {
                return RedirectToAction(nameof(Created));
            }

            return View(assigmentViewModel);
        }

        public IActionResult Give(string groupId)
        {
            return View(new GiveAssignmentInputModel() { GroupId = groupId });
        }

        [HttpPost]
        public async Task<IActionResult> Give(GiveAssignmentInputModel input)
        {
            if(ModelState.IsValid == false)
            {
                return View(input);
            }

            if(input.DueDate.ToUniversalTime() < DateTime.UtcNow.AddHours(1))
            {
                ModelState.AddModelError("DueDate", "Minimum date difference is 1 hour");
                return View(input);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(usersService.IsUserInGroup(userId, input.GroupId) == false)
            {
                return Redirect("/Groups/Index");
            }

            var userRole = usersService.GetRoleInGroup(userId, input.GroupId);
            if(userRole == GroupRole.Student)
            {
                return Redirect("/Groups/Index");
            }

            await assignmentsService.GiveAsync(input);

            return Redirect("/Groups/Index");
        }

        public IActionResult Given()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var givenAssignments = assignmentsService.GetGiven(userId);
            return View(givenAssignments);
        }

        public IActionResult My()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var myAssignments = assignmentsService.GetMyAssignments(userId);
            return View(myAssignments);
        }

        public IActionResult Results(string groupAssignmentId)
        {
            var assignmentResults = assignmentsService.GetResults(groupAssignmentId);
            return View(assignmentResults);
        }

        public IActionResult Solve(string groupAssignmentId)
        {
            var assignmentViewModel = assignmentsService.GetForSolve(groupAssignmentId);
            ViewData["groupAssignmentId"] = groupAssignmentId;
            return View(assignmentViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Solve(SolveAssignmentInputModel input)
        {
            var validationResult = assignmentsService.ValidateSolve(input);
            if(validationResult.IsValid == false)
            {
                var assignmentViewModel = assignmentsService.GetForSolve(input.groupAssignmentId);
                ViewData["groupAssignmentId"] = input.groupAssignmentId;
                return View(assignmentViewModel);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await assignmentsService.ReviewSolveAsync(input, userId);

            return RedirectToAction(nameof(My));
        }

        public IActionResult Preview(string groupAssignmentId, string studentId)
        {
            var preview = assignmentsService.GetPreview(groupAssignmentId, studentId);
            if(preview == null)
            {
                return RedirectToAction(nameof(Created));
            }

            return View(preview);
        }

        public IActionResult Edit(string assignmentId)
        {
            var assignment = assignmentsService.GetForEdit(assignmentId);
            if(assignment == null)
            {
                return RedirectToAction(nameof(Created));
            }

            ViewData["AssignmentId"] = assignmentId;
            return View(assignment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateAssignmentInputModel input, string assignmentId)
        {
            if (ModelState.IsValid == false)
            {
                ViewData["AssignmentId"] = assignmentId;
                return View(input);
            }

            var validationResult = assignmentsService.ValidateAssignment(input);
            if (validationResult.IsValid == false)
            {
                foreach (var key in validationResult.Errors.Keys)
                {
                    foreach (var message in validationResult.Errors[key])
                    {
                        ModelState.AddModelError(key, message);
                    }
                }

                ViewData["AssignmentId"] = assignmentId;
                return View(input);
            }

            await assignmentsService.EditAsync(input, assignmentId);
            return RedirectToAction(nameof(Created));
        }
    }
}

