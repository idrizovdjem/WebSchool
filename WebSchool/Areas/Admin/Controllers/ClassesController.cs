using WebSchool.Models.User;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebSchool.Services.Contracts;
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

        public ClassesController(IClassesService classesService, ISchoolService schoolService, ITeacherService teacherService)
        {
            this.classesService = classesService;
            this.schoolService = schoolService;
            this.teacherService = teacherService;
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

            var schoolId = await this.schoolService.GetSchoolId(this.User);
            if (!this.classesService.IsClassSignatureAvailable(signature, schoolId))
            {
                return RedirectToAction("Index");
            }

            await this.classesService.CreateClass(signature, schoolId);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetClasses()
        {
            var schoolId = await this.schoolService.GetSchoolId(this.User);
            var classes = this.classesService.GetClasses(schoolId);
            return Json(classes);
        }

        public async Task<IActionResult> Information(string signature)
        {
            var schoolId = await this.schoolService.GetSchoolId(this.User);
            var schoolClassModel = this.classesService.GetClassInformation(signature, schoolId);
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

            var schoolId = await this.schoolService.GetSchoolId(this.User);
            await this.classesService.AddStudentsToClass(signature, emails, schoolId);
            return Redirect("/Admin/Classes/Information?signature=" + signature);
        }

        public async Task<IActionResult> Remove(string signature, string email)
        {
            var schoolId = await this.schoolService.GetSchoolId(this.User);
            await this.classesService.Remove(signature, email, schoolId);
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

            var schoolId = await this.schoolService.GetSchoolId(this.User);
            if (!this.classesService.ClassExists(input.Signature, schoolId))
            {
                return Redirect("/Admin/Administration/Teachers");
            }

            await this.classesService.AssignUserToClass(input.Id, input.Signature, schoolId);

            return Redirect("/Admin/Administration/Teachers");
        }

        public async Task<IActionResult> RemoveClass(string classId, string teacherId)
        {
            await this.classesService.RemoveClassFromUser(classId, teacherId);
            return Redirect("/Admin/Administration/Teachers");
        }

        public async Task<IActionResult> GetTeacherClasses(string teacherId)
        {
            var schoolId = await this.schoolService.GetSchoolId(this.User);
            var classes = this.classesService.GetClassesWithoutTeacher(teacherId, schoolId);

            return Json(classes);
        }
    }
}
