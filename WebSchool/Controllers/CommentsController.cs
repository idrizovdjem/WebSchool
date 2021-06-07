using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.Services.Posts;
using WebSchool.ViewModels.Comment;

namespace WebSchool.WebApplication.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class CommentsController : Controller
    {
        private readonly ICommentsService commentsService;
        private readonly IPostsService postsService;

        public CommentsController(
            ICommentsService commentsService, 
            IPostsService postsService)
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

        public IActionResult Edit(int commentId)
        {
            var comment = commentsService.GetForEdit(commentId);
            if(comment == null)
            {
                return Redirect("/Groups/Index");
            }

            return View(comment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCommentInputModel input)
        {
            if(ModelState.IsValid == false)
            {
                return View(input);
            }

            var postId = await commentsService.EditAsync(input);
            if(postId == null)
            {
                return Redirect("/Groups/Index");
            }

            return Redirect("/Posts/Index?postId=" + postId);
        }
    }
}
