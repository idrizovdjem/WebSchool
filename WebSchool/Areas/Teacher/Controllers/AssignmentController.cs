using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebSchool.Models.Assignment;

namespace WebSchool.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class AssignmentController : Controller
    {
        public IActionResult GiveAssignment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GiveAssignment(CreateAssignmentInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return View(input);
            }

            return null;
        }
    }
}
