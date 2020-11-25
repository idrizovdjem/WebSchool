using Microsoft.AspNetCore.Mvc;

namespace WebSchool.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult VerifyPayment()
        {
            return View();
        }
    }
}
