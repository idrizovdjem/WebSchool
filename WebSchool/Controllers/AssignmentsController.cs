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

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userAssignments = assignmentsService.GetUserAssignments(userId);
            return View(userAssignments);
        }
    }
}
