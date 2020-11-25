using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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

        public async Task<IActionResult> VerifyPayment()
        {
            var link = await this.linksService.GenerateLinks("Admin", 1);
            return View(link.FirstOrDefault());
        }
    }
}
