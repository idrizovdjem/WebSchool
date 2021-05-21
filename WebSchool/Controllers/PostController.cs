using WebSchool.Data.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSchool.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System;

namespace WebSchool.Controllers
{
    public class PostController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPostsService postsService;

        public PostController(UserManager<ApplicationUser> userManager, IPostsService postsService)
        {
            this.userManager = userManager;
            this.postsService = postsService;
        }


        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(string content)
        {
            throw new NotImplementedException();

            if (string.IsNullOrWhiteSpace(content))
            {
                return Redirect("/School/Forum");
            }

            var user = await this.userManager.GetUserAsync(this.User);
            // await this.postsService.CreatePost(content, user, user.SchoolId);

            return Redirect("/School/Forum");
        }

    }
}
