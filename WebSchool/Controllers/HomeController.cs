using Microsoft.AspNetCore.Mvc;

namespace WebSchool.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        { 
            if(this.User.Identity.IsAuthenticated)
            {
                return Redirect("/School/id=someSchoolId");
            }

            return View();
        }
    }
}
