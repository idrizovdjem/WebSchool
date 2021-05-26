using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.ViewModels.Group;
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

        [HttpPost]
        public async Task<IActionResult> ChangeGroupName(GroupSettingsViewModel input)
        {
            if(ModelState.IsValid == false)
            {
                return Redirect("/Administration/Index?groupId=" + input.Id);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (groupsService.IsUserInGroup(userId, input.Id) == false)
            {
                return Redirect("/Groups/Index");
            }

            var userRole = usersService.GetRoleInGroup(userId, input.Id);
            if (userRole != GroupRole.Admin)
            {
                return Redirect("/Groups/Index");
            }

            if(groupsService.IsGroupNameAvailable(input.Name) == false)
            {
                return Redirect("/Administration/Index?groupId=" + input.Id);
            }

            await groupsService.ChangeNameAsync(input);
            return Redirect("/Administration/Index?groupId=" + input.Id);
        }
    }
}
