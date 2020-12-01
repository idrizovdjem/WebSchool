using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSchool.Services.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace WebSchool.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ClassesController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IClassesService classesService;
        private readonly ISchoolService schoolService;

        public ClassesController(IUsersService usersService, IClassesService classesService, ISchoolService schoolService)
        {
            this.usersService = usersService;
            this.classesService = classesService;
            this.schoolService = schoolService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateClass(string signature)
        {
            if (string.IsNullOrWhiteSpace(signature))
            {
                return RedirectToAction("Index");
            }

            var user = await this.usersService.GetUser(this.User);
            var schoolId = this.schoolService.GetSchoolIdByUser(user);
            if (!this.classesService.IsClassSignatureAvailable(signature, schoolId))
            {
                return RedirectToAction("Index");
            }

            await this.classesService.CreateClass(signature, schoolId);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetClasses()
        {
            var user = await this.usersService.GetUser(this.User);
            var schoolId = this.schoolService.GetSchoolIdByUser(user);
            var classes = this.classesService.GetClasses(schoolId);
            return Json(classes);
        }
    }
}
