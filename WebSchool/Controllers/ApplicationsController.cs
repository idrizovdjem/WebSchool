using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.Services.Groups;
using WebSchool.Services.Common;
using WebSchool.Common.Constants;
using WebSchool.Common.Enumerations;
using WebSchool.Services.Administration;

namespace WebSchool.WebApplication.Controllers
{
    [Authorize]
    public class ApplicationsController : Controller
    {
        private readonly IGroupsService groupsService;
        private readonly IApplicationsService applicationsService;
        private readonly INotificationsService notificationsService;
        private readonly IUsersService usersService;

        public ApplicationsController(
            IGroupsService groupsService, 
            IApplicationsService applicationsService,
            INotificationsService notificationsService,
            IUsersService usersService)
        {
            this.groupsService = groupsService;
            this.applicationsService = applicationsService;
            this.notificationsService = notificationsService;
            this.usersService = usersService;
        }

        public async Task<IActionResult> Apply(string groupId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (groupsService.GroupExists(groupId) == false)
            {
                return Redirect("/Browse/Index");
            }

            var applicationStatus = applicationsService.GetApplicationStatus(userId, groupId);
            if (applicationStatus != ApplicationStatus.NotApplied)
            {
                return Redirect("/Browse/Index");
            }

            await applicationsService.ApplyAsync(userId, groupId);
            return Redirect("/Browse/Index");
        }
    }
}
