using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.Services.Contracts;
using WebSchool.Common.Enumerations;

namespace WebSchool.WebApplication.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
        private readonly IGroupsService groupsService;
        private readonly IUsersService usersService;

        public AdministrationController(IGroupsService groupsService, IUsersService usersService)
        {
            this.groupsService = groupsService;
            this.usersService = usersService;
        }

        public IActionResult Index(string groupId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(groupsService.IsUserInGroup(userId, groupId) == false)
            {
                return Redirect("/Groups/Index");
            }

            var userRole = usersService.GetRoleInGroup(userId, groupId);
            if(userRole != GroupRole.Admin)
            {
                return Redirect("/Groups/Index");
            }

            var viewModel = groupsService.GetSettings(groupId);
            return View(viewModel);
        }
    }
}
