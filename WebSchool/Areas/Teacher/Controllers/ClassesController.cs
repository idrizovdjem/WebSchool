using WebSchool.Data.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSchool.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace WebSchool.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class ClassesController : Controller
    {
        private readonly IClassesService classesService;
        private readonly UserManager<ApplicationUser> userManager;

        public ClassesController(IClassesService classesService, UserManager<ApplicationUser> userManager)
        {
            this.classesService = classesService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> GetClasses()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var classes = this.classesService.GetTeacherAssignedClasses(user.Id);

            return Json(classes);
        }
    }
}
