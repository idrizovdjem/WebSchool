using WebSchool.Data.Models;
using System.Threading.Tasks;
using WebSchool.ViewModels.School;
using Microsoft.AspNetCore.Mvc;
using WebSchool.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace WebSchool.Controllers
{
    public class SchoolController : Controller
    {
        private readonly ISchoolService schoolService;
        private readonly UserManager<ApplicationUser> userManager;

        public SchoolController(ISchoolService schoolService, UserManager<ApplicationUser> userManager)
        {
            this.schoolService = schoolService;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateSchoolInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return View(input);
            }

            if (!this.schoolService.IsSchoolNameAvailable(input.Name))
            {
                this.ModelState.AddModelError("School name", "School name is already in use");
                return View(input);
            }

            var school = new School()
            {
                Name = input.Name,
                ImageUrl = input.ImageUrl,
            };

            await this.schoolService.CreateAsync(school);
            var user = await this.userManager.GetUserAsync(this.User);
            await this.schoolService.AssignUserToSchool(user, school.Id);

            return RedirectToAction("Forum");
        }

        [Authorize]
        public async Task<IActionResult> Forum(int page = 1)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var school = this.schoolService.GetSchool(user, page);

            return View(school);
        }
    }
}
