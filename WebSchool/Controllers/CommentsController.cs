using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.ViewModels.Comment;
using WebSchool.Services.Contracts;

namespace WebSchool.WebApplication.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class CommentsController : Controller
    {
        private readonly ICommentsService commentsService;
        private readonly IPostsService postsService;

        public CommentsController(ICommentsService commentsService, IPostsService postsService)
        {
            this.commentsService = commentsService;
            this.postsService = postsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentInputModel input)
        {
            if(ModelState.IsValid == false)
            {
                return Redirect($"/Posts?postId={input.PostId}");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var post = postsService.GetById(userId, input.PostId);
            if(post == null)
            {
                return Redirect($"/Posts?postId={input.PostId}");
            }

            await this.commentsService.CreateAsync(post.Id, input.Content, userId);

            return Redirect($"/Posts?postId={input.PostId}");
        }
    }
}
