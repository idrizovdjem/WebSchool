using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.ViewModels.Post;
using WebSchool.Services.Contracts;

namespace WebSchool.WebApplication.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class PostsController : Controller
    {
        private readonly IGroupsService groupsService;
        private readonly IPostsService postsService;

        public PostsController(IGroupsService groupsService, IPostsService postsService)
        {
            this.groupsService = groupsService;
            this.postsService = postsService;
        }

        public IActionResult Create(string groupId)
        {
            var groupName = groupsService.GetName(groupId);
            if(groupName == null)
            {
                return Redirect("/Groups/Index");
            }

            ViewData["GroupId"] = groupId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostInputModel input)
        {
            if(ModelState.IsValid == false)
            {
                return View(input);
            }

            var groupName = groupsService.GetName(input.GroupId);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(groupName == null)
            {
                return Redirect("/Groups/Index");
            }

            if(groupName == "Global Group")
            {
                await postsService.CreateAsync(input, userId);
                return Redirect("/Groups/Index");
            }

            var isUserInGroup = groupsService.IsUserInGroup(userId, input.GroupId);
            if(isUserInGroup == false)
            {
                return Redirect("/Groups/Index");
            }

            await postsService.CreateAsync(input, userId);
            return Redirect($"/Groups/Index?groupName={groupName}");
        }

        public IActionResult Index(string postId)
        {
            var post = postsService.GetById(postId);
            if(post == null)
            {
                return Redirect("/Groups/Index");
            }

            return View(post);
        }
    }
}
