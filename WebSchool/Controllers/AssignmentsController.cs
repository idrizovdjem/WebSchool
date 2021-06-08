using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.Services.Assignments;
using WebSchool.ViewModels.Assignment;

namespace WebSchool.WebApplication.Controllers
{
    [Authorize]
    public class AssignmentsController : Controller
    {
        private readonly IAssignmentsService assignmentsService;

        public AssignmentsController(IAssignmentsService assignmentsService)
        {
            this.assignmentsService = assignmentsService;
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

            assigmentViewModel.AllPoints = assigmentViewModel.Questions.Sum(q => q.Points);
            return View(assigmentViewModel);
        }

        public IActionResult Give(string groupId)
        {
            return View();
        }
    }
}

