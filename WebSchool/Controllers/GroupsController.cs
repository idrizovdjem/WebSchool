using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.Services.Contracts;
using System.Security.Claims;

namespace WebSchool.Web.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private readonly IGroupsService groupsService;

        public GroupsController(IGroupsService groupsService)
        {
            this.groupsService = groupsService;
        }

        public IActionResult Index(string groupName = "Global Group")
        {
            var groupViewModel = groupsService.GetGroupContent(groupName);
            return View(groupViewModel);
        }

        [Route("/Groups/api/GetUserGroups")]
        public IActionResult GetUserGroups()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var groupNames = groupsService.GetUserGroups(userId);
            return Json(groupNames);
        }
    }
}
