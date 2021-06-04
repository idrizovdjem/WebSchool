using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using WebSchool.Services.Contracts;

namespace WebSchool.WebApplication.Controllers.ApiControllers
{
    public class ApiCommentsController : Controller
    {
        private readonly ICommentsService commentsService;

        public ApiCommentsController(
            ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        public async Task<IActionResult> Remove(int commentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var removeResult = await commentsService.RemoveAsync(userId, commentId);
            return Json(removeResult);
        }
    }
}
