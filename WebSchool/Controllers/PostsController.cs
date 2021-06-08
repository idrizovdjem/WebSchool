using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.ViewModels.Post;
using WebSchool.Services.Posts;
using WebSchool.Services.Groups;
using WebSchool.Services.Common;
using WebSchool.Common.Constants;

namespace WebSchool.WebApplication.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class PostsController : Controller
    {
        private readonly IGroupsService groupsService;
        private readonly IPostsService postsService;
        private readonly IUsersService usersService;

        public PostsController(
            IGroupsService groupsService, 
            IPostsService postsService, 
            IUsersService usersService)
        {
            this.groupsService = groupsService;
            this.postsService = postsService;
            this.usersService = usersService;
        }

        public IActionResult Create(string groupId)
        {
            var groupName = groupsService.GetName(groupId);
            if(groupName == null)
            {
                return Redirect("/Groups/Index");
            }

            ViewData["GroupId"] = groupId;
            ViewData["GroupName"] = groupName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostInputModel input)
        {
            if(ModelState.IsValid == false)
            {
                ViewData["GroupId"] = input.GroupId;
                return View(input);
            }

            var groupName = groupsService.GetName(input.GroupId);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(groupName == null)
            {
                return Redirect("/Groups/Index");
            }

            if(groupName == GroupConstants.GlobalGroupName)
            {
                await postsService.CreateAsync(input, userId);
                return Redirect("/Groups/Index");
            }

            var isUserInGroup = usersService.IsUserInGroup(userId, input.GroupId);
            if(isUserInGroup == false)
            {
                return Redirect("/Groups/Index");
            }

            await postsService.CreateAsync(input, userId);
            return Redirect($"/Groups/Index?groupId={input.GroupId}");
        }

        public IActionResult Index(string postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var post = postsService.GetById(userId, postId);
            if(post == null)
            {
                return Redirect("/Groups/Index");
            }

            return View(post);
        }

        public async Task<IActionResult> Remove(string postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userValidation = usersService.ValidatePostRemove(userId, postId);
            if(userValidation == false)
            {
                return Redirect("/Groups/Index");
            }

            await postsService.RemoveAsync(postId);
            return Redirect("/Groups/Index");
        }

        public IActionResult Edit(string postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var post = postsService.GetForEdit(userId, postId);
            if(post == null)
            {
                return Redirect("/Groups/Index");
            }

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPostInputModel input)
        {
            if(ModelState.IsValid == false)
            {
                return View(input);
            }

            await postsService.EditAsync(input);
            return RedirectToAction(nameof(Index), new { postId = input.Id });
        }
    }
}
