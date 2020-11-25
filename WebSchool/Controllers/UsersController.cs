using Microsoft.AspNetCore.Mvc;
using WebSchool.Models.RegistrationLink;

namespace WebSchool.Controllers
{
    public class UsersController : Controller
    {
        [HttpPost]
        public IActionResult RegisterForm(RegistrationLinkModel input)
        {
            return View(input);
        }
    }
}
