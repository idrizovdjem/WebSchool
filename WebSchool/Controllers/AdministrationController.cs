using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.ViewModels.Group;
using WebSchool.Services.Contracts;

namespace WebSchool.WebApplication.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
        private readonly IGroupsService groupsService;
        private readonly IUsersService usersService;
        private readonly IApplicationsService applicationsService;
        private readonly IAdministrationService administrationService;

        public AdministrationController(IGroupsService groupsService, IUsersService usersService, IApplicationsService applicationsService, IAdministrationService administrationService)
        {
            this.groupsService = groupsService;
            this.usersService = usersService;
            this.applicationsService = applicationsService;
            this.administrationService = administrationService;
        }

        public IActionResult Index(string groupId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (administrationService.ValidateIfUserIsAdmin(userId, groupId) == false)
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
            if(administrationService.ValidateIfUserIsAdmin(userId, input.Id) == false)
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

        public IActionResult Applications(string groupId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(administrationService.ValidateIfUserIsAdmin(userId, groupId) == false)
            {
                return Redirect("/Groups/Index");
            }


            ViewData["GroupId"] = groupId;
            var applications = applicationsService.GetApplications(groupId);
            return View(applications);
        }
    }
}
