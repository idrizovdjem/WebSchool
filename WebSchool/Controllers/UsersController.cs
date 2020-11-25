using Microsoft.AspNetCore.Mvc;
using WebSchool.Models.RegistrationLink;

namespace WebSchool.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Register(string id)
        {
            return View();
        }
    }
}