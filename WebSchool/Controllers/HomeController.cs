using Microsoft.AspNetCore.Mvc;

namespace WebSchool.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
