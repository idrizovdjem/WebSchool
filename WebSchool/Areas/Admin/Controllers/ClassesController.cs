using WebSchool.Data.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSchool.ViewModels.User;
using System.Collections.Generic;
using WebSchool.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace WebSchool.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ClassesController : Controller
    {
        private readonly IClassesService classesService;
        private readonly ISchoolService schoolService;
        private readonly ITeacherService teacherService;
        private readonly UserManager<ApplicationUser> userManager;

        public ClassesController(IClassesService classesService, ISchoolService schoolService, ITeacherService teacherService, UserManager<ApplicationUser> userManager)
        {
            this.classesService = classesService;
            this.schoolService = schoolService;
            this.teacherService = teacherService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClass(string signature)
        {
            if (string.IsNullOrWhiteSpace(signature))
            {
                return RedirectToAction("Index");
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (!this.classesService.IsClassSignatureAvailable(signature, user.SchoolId))
            {
                return RedirectToAction("Index");
            }

            await this.classesService.CreateClass(signature, user.SchoolId);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetClasses()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var classes = this.classesService.GetClasses(user.SchoolId);
            return Json(classes);
        }

        public async Task<IActionResult> Information(string signature)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var schoolClassModel = this.classesService.GetClassInformation(signature, user.SchoolId);
            return View(schoolClassModel);
        }

        public IActionResult AddStudents(string signature)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudents(string signature, List<string> emails)
        {
            if (emails == null || emails.Count == 0)
            {
                return Redirect("/Admin/Classes/Information?signature=" + signature);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            await this.classesService.AddStudentsToClass(signature, emails, user.SchoolId);
            return Redirect("/Admin/Classes/Information?signature=" + signature);
        }

        public async Task<IActionResult> Remove(string signature, string email)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            await this.classesService.Remove(signature, email, user.SchoolId);
            return Redirect($"/Admin/Classes/Information?signature={signature}");
        }

        public IActionResult AssignToClass(string id)
        {
            var teacher = this.teacherService.GetTeacher(id);
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignToClass(AssignClassToTeacherInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return Redirect("/Admin/Administration/Teachers");
            }

            var user = await this.userManager.GetUserAsync(this.User);
            if (!this.classesService.ClassExists(input.Signature, user.SchoolId))
            {
                return Redirect("/Admin/Administration/Teachers");
            }

            await this.classesService.AssignUserToClass(input.Id, input.Signature, user.SchoolId);

            return Redirect("/Admin/Administration/Teachers");
        }

        public async Task<IActionResult> RemoveClass(string classId, string teacherId)
        {
            await this.classesService.RemoveClassFromUser(classId, teacherId);
            return Redirect("/Admin/Administration/Teachers");
        }

        public async Task<IActionResult> GetTeacherClasses(string teacherId)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var classes = this.classesService.GetClassesWithoutTeacher(teacherId, user.SchoolId);

            return Json(classes);
        }
    }
}
