using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.Services.Contracts;
using System.Threading.Tasks;
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

            return View(input);
        }

        public IActionResult Created()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var createdAssignments = assignmentsService.GetCreated(userId);
            return View(createdAssignments);
        }
    }
}
