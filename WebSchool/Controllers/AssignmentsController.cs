using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.Services.Contracts;

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

        public IActionResult Created()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var createdAssignments = assignmentsService.GetCreated(userId);
            return View(createdAssignments);
        }
    }
}
