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
        private readonly IBrowseService browseService;

        public ApiGroupsController(IGroupsService groupsService, IBrowseService browseService)
        {
            this.groupsService = groupsService;
            this.browseService = browseService;
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
            var groups = browseService.GetGroupsContainingName(userId, groupName);
            return Json(groups);
        }

        public IActionResult GetMostPopular()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var groups = browseService.GetMostPopular(userId);
            return Json(groups);
        }

    }
}
