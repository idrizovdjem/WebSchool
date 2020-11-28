using WebSchool.Data.Models;
using WebSchool.Models.Post;
using System.Threading.Tasks;
using WebSchool.Models.School;
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
        private readonly IPostsService postsService;

        public SchoolController(ISchoolService schoolService, UserManager<ApplicationUser> userManager, IPostsService postsService)
        {
            this.schoolService = schoolService;
            this.userManager = userManager;
            this.postsService = postsService;
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
            await this.schoolService.AssignUserToSchool(user.Id, school.Id);

            return RedirectToAction("Forum");
        }

        [Authorize]
        public async Task<IActionResult> Forum(int page = 1)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var school = this.schoolService.GetSchool(user, page);

            return View(school);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(CreatePostInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return RedirectToAction("Forum");
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var schoolId = this.schoolService.GetSchoolIdByUser(user);
            await this.postsService.CreatePost(input, user, schoolId);

            return RedirectToAction("Forum");
        }
    }
}
