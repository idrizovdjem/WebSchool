using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebSchool.Controllers
{
    public class SchoolController : Controller
    {
        //[Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
    }
}
