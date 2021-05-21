using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.ViewModels.Post;
using WebSchool.Services.Contracts;

namespace WebSchool.WebApplication.Controllers
{
    
    public class PostsController : Controller
    {
        private readonly IGroupsService groupsService;
        private readonly IPostsService postsService;

        public PostsController(IGroupsService groupsService, IPostsService postsService)
        {
            this.groupsService = groupsService;
            this.postsService = postsService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreatePostInputModel input)
        {
            if(ModelState.IsValid == false)
            {
                return Redirect("/Groups/Index");
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
                return Redirect($"/Groups/Index?Name={groupName}");
            }

            var isUserInGroup = groupsService.IsUserInGroup(userId, input.GroupId);
            if(isUserInGroup == false)
            {
                return Redirect("/Groups/Index");
            }

            await postsService.CreateAsync(input, userId);
            return Redirect($"/Groups/Index?Name={groupName}");
        }
    }
}
