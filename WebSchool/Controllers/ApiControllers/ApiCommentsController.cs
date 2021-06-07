using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using WebSchool.Services.Posts;
using WebSchool.Services.Common;

namespace WebSchool.WebApplication.Controllers.ApiControllers
{
    public class ApiCommentsController : Controller
    {
        private readonly ICommentsService commentsService;
        private readonly IUsersService usersService;

        public ApiCommentsController(
            ICommentsService commentsService,
            IUsersService usersService)
        {
            this.commentsService = commentsService;
            this.usersService = usersService;
        }

        public async Task<IActionResult> Remove(string commentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(usersService.ValidateCommentRemove(userId, commentId) == false)
            {
                return Json(false);
            }

            var removeResult = await commentsService.RemoveAsync(userId, commentId);
            return Json(removeResult);
        }
    }
}
