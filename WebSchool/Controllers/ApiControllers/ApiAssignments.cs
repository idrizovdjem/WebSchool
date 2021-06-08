using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.Services.Assignments;
using System.Security.Claims;

namespace WebSchool.WebApplication.Controllers.ApiControllers
{
    [Authorize]
    [ApiController]
    [Route("/apiAssignments/[action]")]
    public class ApiAssignments : Controller
    {
        private readonly IAssignmentsService assignmentsService;

        public ApiAssignments(IAssignmentsService assignmentsService)
        {
            this.assignmentsService = assignmentsService;
        }

        public IActionResult All()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var assignments = assignmentsService.GetCreated(userId);
            return Json(assignments);
        }
    }
}
