using WebSchool.Data.Models;
using WebSchool.ViewModels.Post;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSchool.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace WebSchool.Controllers
{
    public class PostController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ISchoolService schoolService;
        private readonly IPostsService postsService;

        public PostController(UserManager<ApplicationUser> userManager, ISchoolService schoolService, IPostsService postsService)
        {
            this.userManager = userManager;
            this.schoolService = schoolService;
            this.postsService = postsService;
        }


        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(CreatePostInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return Redirect("/School/Forum");
            }

            var user = await this.userManager.GetUserAsync(this.User);
            await this.postsService.CreatePost(input, user, user.SchoolId);

            return Redirect("/School/Forum");
        }

    }
}
