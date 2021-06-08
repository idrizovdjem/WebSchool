using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.Services.Groups;
using WebSchool.ViewModels.Group;
using WebSchool.Common.Constants;
using WebSchool.Common.Enumerations;

namespace WebSchool.Web.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class GroupsController : Controller
    {
        private readonly IGroupsService groupsService;

        public GroupsController(IGroupsService groupsService)
        {
            this.groupsService = groupsService;
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
                    groupId = groupsService.GetIdByName(GroupConstants.GlobalGroupName);
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
            var groupId = await groupsService.CreateAsync(userId, input.Name);
            await groupsService.AddUserToGroupAsync(userId, groupId, GroupRole.Admin);

            return Redirect($"/Groups/Index/groupName={groupId}");
        }
    }
}
