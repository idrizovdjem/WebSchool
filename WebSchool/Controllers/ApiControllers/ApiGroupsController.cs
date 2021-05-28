using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.Services.Contracts;

namespace WebSchool.WebApplication.Controllers.ApiControllers
{
    [Authorize]
    public class ApiGroupsController : Controller
    {
        private readonly IGroupsService groupsService;

        public ApiGroupsController(IGroupsService groupsService)
        {
            this.groupsService = groupsService;
        }

        public IActionResult GetUserGroups()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var groupNames = groupsService.GetUserGroups(userId);
            return Json(groupNames);
        }

        public IActionResult IsNameValid(string groupName)
        {
            if(groupName.Length < 5 || groupName.Length > 250)
            {
                return Json(false);
            }

            var isNameAvailable = groupsService.IsGroupNameAvailable(groupName);
            return Json(isNameAvailable);
        }

        public IActionResult GetGroupsByName(string groupName)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var groups = groupsService.GetGroupsContainingName(userId, groupName);
            return Json(groups);
        }
    }
}
