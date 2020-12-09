using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebSchool.Services.Contracts;
using System.Threading.Tasks;

namespace WebSchool.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class ClassesController : Controller
    {
        private readonly IClassesService classesService;
        private readonly IUsersService usersService;

        public ClassesController(IClassesService classesService, IUsersService usersService)
        {
            this.classesService = classesService;
            this.usersService = usersService;
        }

        public async Task<IActionResult> GetClasses()
        {
            var user = await this.usersService.GetUser(this.User);
            var classes = this.classesService.GetTeacherAssignedClasses(user.Id);

            return Json(classes);
        }
    }
}
