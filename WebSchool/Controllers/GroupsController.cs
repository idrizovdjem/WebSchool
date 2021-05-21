using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.Services.Contracts;

namespace WebSchool.Web.Controllers
{
    public class GroupsController : Controller
    {
        private readonly IGroupsService groupsService;

        public GroupsController(IGroupsService groupsService)
        {
            this.groupsService = groupsService;
        }

        [Authorize]
        public IActionResult Index(string groupName = "Global Group")
        {
            var groupViewModel = groupsService.GetGroupContent(groupName);
            return View(groupViewModel);
        }
    }
}
