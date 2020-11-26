using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSchool.Services.Contracts;

namespace WebSchool.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILinksService linksService;
        private readonly IEmailSenderService emailSenderService;

        public HomeController(ILinksService linksService,
            IEmailSenderService emailSenderService)
        {
            this.linksService = linksService;
            this.emailSenderService = emailSenderService;
        }

        public IActionResult Index()
        { 
            if(this.User.Identity.IsAuthenticated)
            {
                return Redirect("/School/id=someSchoolId");
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

            var link = await this.linksService.GenerateAdminLink(email);
            await this.emailSenderService.SendRegistrationEmail(link.Id, email);

            return View("SuccessfullEmail");
        }
    }
}
