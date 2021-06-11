using Microsoft.AspNetCore.Mvc;

namespace WebSchool.WebApplication.Controllers
{
    public class NotificationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
