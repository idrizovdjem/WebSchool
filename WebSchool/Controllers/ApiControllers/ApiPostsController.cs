using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.Services.Contracts;

namespace WebSchool.WebApplication.Controllers.ApiControllers
{
    [Authorize]
    public class ApiPostsController : Controller
    {
        private readonly IPostsService postsService;

        public ApiPostsController(
            IPostsService postsService)
        {
            this.postsService = postsService;
        }

        public async Task<IActionResult> Remove(string postId)
        {
            var removeResult = await postsService.RemoveAsync(postId);
            return Json(removeResult);
        }
    }
}
