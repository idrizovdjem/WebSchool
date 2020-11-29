using WebSchool.Data.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSchool.Models.Comment;
using WebSchool.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace WebSchool.Controllers
{
    public class CommentController : Controller
    {
        private readonly IPostsService postsService;
        private readonly ISchoolService schoolService;
        private readonly ICommentsService commentsService;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentController(IPostsService postsService, UserManager<ApplicationUser> userManager, ISchoolService schoolService, ICommentsService commentsService)
        {
            this.userManager = userManager;
            this.postsService = postsService;
            this.schoolService = schoolService;
            this.commentsService = commentsService;
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment(CreateCommentInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return Redirect("/School/Forum");
            }

            var post = this.postsService.GetPost(input.PostId);
            if (post == null)
            {
                return Redirect("/School/Forum");
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var schoolId = this.schoolService.GetSchoolIdByUser(user);
            if (post.SchoolId != schoolId)
            {
                return Redirect("/School/Forum");
            }

            await this.commentsService.AddCommentAsync(post.Id, input.Content, user.Id);

            return Redirect("/School/Forum");
        }

        [Authorize]
        public IActionResult GetComments(string postId)
        {
            var comments = this.commentsService.GetComments(postId);
            return Json(comments);
        }
    }
}
