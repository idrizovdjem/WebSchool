using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.Services.Common;
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
        private readonly IUsersService usersService;

        public GroupsController(
            IGroupsService groupsService,
            IUsersService usersService)
        {
            this.groupsService = groupsService;
            this.usersService = usersService;
        }

        public IActionResult Index(string groupId)
        {
            if(groupId == null && HttpContext.Request.Cookies.ContainsKey("LastVisited"))
            {
                groupId = HttpContext.Request.Cookies["LastVisited"];
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (groupId == null || usersService.IsUserInGroup(userId, groupId) == false)
            {
                groupId = groupsService.GetIdByName(GroupConstants.GlobalGroupName);
            }

            var groupViewModel = groupsService.GetGroupContent(userId, groupId);
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

            return RedirectToAction(nameof(Index), new { groupId });
        }

        public IActionResult JoinedGroups()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var joinedGroups = groupsService.GetUserGroups(userId);
            return View(joinedGroups);
        }

        public IActionResult Results(string groupId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var assignmentResult = groupsService.GetResults(groupId, userId);
            return View(assignmentResult);
        }
    }
}
