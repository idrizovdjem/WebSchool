using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.Services.Contracts;
using WebSchool.Common.Enumerations;

namespace WebSchool.WebApplication.Controllers
{
    [Authorize]
    public class ApplicationsController : Controller
    {
        private readonly IGroupsService groupsService;
        private readonly IApplicationsService applicationsService;

        public ApplicationsController(IGroupsService groupsService, IApplicationsService applicationsService)
        {
            this.groupsService = groupsService;
            this.applicationsService = applicationsService;
        }

        public async Task<IActionResult> Apply(string groupId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (groupsService.GroupExists(groupId) == false)
            {
                return Redirect("/Groups/Index");
            }

            if(groupsService.IsUserInGroup(userId, groupId))
            {
                return Redirect("/Groups/Index");
            }

            if(applicationsService.GetApplicationStatus(userId, groupId) != ApplicationStatus.NotApplied)
            {
                return Redirect("/Groups/Index");
            }

            await applicationsService.ApplyAsync(userId, groupId);

            return Redirect("/Groups/Index");
        }
    }
}
