using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSchool.Services.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace WebSchool.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SubjectController : Controller
    {
        private readonly ISubjectService subjectService;
        private readonly ISchoolService schoolService;

        public SubjectController(ISubjectService subjectService, ISchoolService schoolService)
        {
            this.subjectService = subjectService;
            this.schoolService = schoolService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubject(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                return RedirectToAction("Index");
            }

            var schoolId = await this.schoolService.GetSchoolId(this.User);

            if (this.subjectService.DoesSubjectExists(subject, schoolId))
            {
                return RedirectToAction("Index");
            }

            await this.subjectService.CreateSubject(subject, schoolId);

            return RedirectToAction("Index");
        }
    }
}
