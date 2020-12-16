using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSchool.Services.Contracts;
using Microsoft.Extensions.Configuration;

namespace WebSchool.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILinksService linksService;
        private readonly IEmailsService emailsService;
        private readonly IConfiguration configuration;

        public HomeController(ILinksService linksService,
            IEmailsService emailsService,
            IConfiguration configuration)
        {
            this.linksService = linksService;
            this.emailsService = emailsService;
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return Redirect("/School/Forum");
            }

            return View();
        }

        public IActionResult CreateUser()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return Redirect("/School/Forum");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(string email)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return Redirect("/School/Forum");
            }

            if (!this.emailsService.IsEmailAvailable(email))
            {
                this.ModelState.AddModelError("Email", "Email address is already in use");
                return View();
            }
            var link = await this.linksService.GenerateAdminLink(email);
            await this.emailsService.SendRegistrationEmail(link.Id, email, this.configuration["SendGripApi"]);

            return View("SuccessRegistration");
        }
    }
}
