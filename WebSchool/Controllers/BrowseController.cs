using Microsoft.AspNetCore.Mvc;

namespace WebSchool.WebApplication.Controllers
{
    public class BrowseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
