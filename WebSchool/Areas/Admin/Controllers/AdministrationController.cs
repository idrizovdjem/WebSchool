using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebSchool.Areas.Admin.Controllers
{
    public class AdministrationController : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Panel()
        {
            return View();
        }
    }
}
