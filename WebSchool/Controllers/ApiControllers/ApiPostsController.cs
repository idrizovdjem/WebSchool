using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.Services.Contracts;

namespace WebSchool.WebApplication.Controllers.ApiControllers
{
    [Authorize]
    public class ApiPostsController : Controller
    {
        private readonly IPostsService postsService;

        public ApiPostsController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        public async Task<IActionResult> Remove(string postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var removeResult = await postsService.RemoveAsync(userId, postId);
            return Json(removeResult);
        }
    }
}
