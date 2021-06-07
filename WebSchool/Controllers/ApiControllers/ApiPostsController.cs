using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.Services.Posts;
using WebSchool.Services.Common;

namespace WebSchool.WebApplication.Controllers.ApiControllers
{
    [Authorize]
    public class ApiPostsController : Controller
    {
        private readonly IPostsService postsService;
        private readonly IUsersService usersService;

        public ApiPostsController(
            IPostsService postsService,
            IUsersService usersService)
        {
            this.postsService = postsService;
            this.usersService = usersService;
        }

        public async Task<IActionResult> Remove(string postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(usersService.ValidatePostRemove(userId, postId) == false)
            {
                return Json(false);
            }

            var removeResult = await postsService.RemoveAsync(postId);
            return Json(removeResult);
        }
    }
}
