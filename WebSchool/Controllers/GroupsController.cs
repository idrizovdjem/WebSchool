using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.ViewModels.Group;
using WebSchool.Services.Contracts;

namespace WebSchool.Web.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class GroupsController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IGroupsService groupsService;

        public GroupsController(
            IGroupsService groupsService,
            IUsersService usersService)
        {
            this.groupsService = groupsService;
            this.usersService = usersService;
        }

        public IActionResult Index(string groupId)
        {
            if(groupId == null)
            {
                if(HttpContext.Request.Cookies.ContainsKey("LastVisited"))
                {
                    groupId = HttpContext.Request.Cookies["LastVisited"];
                }
                else
                {
                    groupId = groupsService.GetIdByName("Global Group");
                }
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var groupViewModel = groupsService.GetGroupContent(userId, groupId);

            if(groupViewModel == null)
            {
                return Redirect("/Groups/Index");
            }

            HttpContext.Response.Cookies.Append("LastVisited", groupId);
            return View(groupViewModel);
        }        

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGroupInputModel input)
        {
            if(ModelState.IsValid == false)
            {
                return View(input);
            }

            if(groupsService.IsGroupNameAvailable(input.Name) == false)
            {
                ModelState.AddModelError("Name", "This group name is already taken");
                return View(input);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var createGroup = await groupsService.CreateAsync(userId, input.Name);

            return Redirect($"/Groups/Index/groupName={createGroup.Id}");
        }
    }
}
