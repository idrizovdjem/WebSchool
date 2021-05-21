using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSchool.Services.Contracts;
using Microsoft.Extensions.Configuration;
using System;

namespace WebSchool.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;

        public HomeController(IConfiguration configuration)
        {
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
            throw new NotImplementedException();

            //if (this.User.Identity.IsAuthenticated)
            //{
            //    return Redirect("/School/Forum");
            //}

            //if (!this.emailsService.IsEmailAvailable(email))
            //{
            //    this.ModelState.AddModelError("Email", "Email address is already in use");
            //    return View();
            //}
            //var link = await this.linksService.GenerateAdminLink(email);
            //await this.emailsService.SendRegistrationEmail(link.Id, email, this.configuration["SendGripApi"]);

            //return View("SuccessRegistration");
        }
    }
}
