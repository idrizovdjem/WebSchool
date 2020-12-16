using WebSchool.Data.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSchool.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using WebSchool.ViewModels.RegistrationLink;
using Microsoft.Extensions.Configuration;

namespace WebSchool.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly ILinksService linksService;
        private readonly ISchoolService schoolService;
        private readonly IEmailsService emailsService;
        private readonly IUsersService usersService;
        private readonly ITeacherService teacherService;
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(ILinksService linksService, UserManager<ApplicationUser> userManager, ISchoolService schoolService, IEmailsService emailsService, IUsersService usersService, ITeacherService teacherService, IConfiguration configuration)
        {
            this.linksService = linksService;
            this.userManager = userManager;
            this.schoolService = schoolService;
            this.emailsService = emailsService;
            this.usersService = usersService;
            this.teacherService = teacherService;
            this.configuration = configuration;
        }

        public IActionResult Panel()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateLinks(GenerateLinksInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return RedirectToAction("Panel");
            }

            if (!this.linksService.IsRoleValid(input.Role))
            {
                return RedirectToAction("Panel");
            }

            var emails = input.Emails.Split(", ");
            if (emails.Length < 1)
            {
                return RedirectToAction("Panel");
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var links = await this.linksService.GenerateLinks(input.Role, user.Email, user.SchoolId, emails);
            foreach (var link in links)
            {
                await this.emailsService.SendRegistrationEmail(link.Id, link.To, this.configuration["SendGripApi"]);
            }

            return RedirectToAction("Panel");
        }

        public async Task<IActionResult> GetLinks()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var links = this.linksService.GetGeneratedLinks(user.Email);
            return Json(links);
        }

        public async Task<IActionResult> DeleteLink(string id)
        {
            await this.linksService.Delete(id);
            return RedirectToAction("Panel");
        }

        public async Task<IActionResult> Users()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var users = this.usersService.GetUsersTable(user.SchoolId);
            return View(users);
        }

        public async Task<IActionResult> Teachers()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var teachers = this.teacherService.GetTeachers(user.SchoolId);
            return View(teachers);
        }
    }
}
