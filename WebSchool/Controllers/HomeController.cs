using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSchool.Services.Contracts;

namespace WebSchool.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILinksService linksService;
        private readonly IEmailsService emailsService;

        public HomeController(ILinksService linksService,
            IEmailsService emailsService)
        {
            this.linksService = linksService;
            this.emailsService = emailsService;
        }

        public IActionResult Index()
        { 
            if(this.User.Identity.IsAuthenticated)
            {
                return Redirect("/School/Forum");
            }

            return View();
        }

        public IActionResult CreateUser()
        {
            if(this.User.Identity.IsAuthenticated)
            {
                return Redirect("/School/Forum");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(string email)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return Redirect("/School/Forum");
            }

            if(!this.emailsService.IsEmailAvailable(email))
            {
                this.ModelState.AddModelError("Email", "Email address is already in use");
                return View();
            }
            var link = await this.linksService.GenerateAdminLink(email);
            await this.emailsService.SendRegistrationEmail(link.Id, email);

            return View("SuccessfullEmail");
        }

        // remove
        // just for testing
        public IActionResult SuccessfullEmail()
        {
            return View();
        }
    }
}
