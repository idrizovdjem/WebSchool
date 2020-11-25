using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSchool.Services.Contracts;

namespace WebSchool.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ILinksService linksService;

        public PaymentController(ILinksService linksService)
        {
            this.linksService = linksService;
        }

        public IActionResult VerifyPayment()
        {
            return View();
        }
    }
}
