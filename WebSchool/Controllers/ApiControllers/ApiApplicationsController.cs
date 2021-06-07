using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.Services.Groups;
using WebSchool.Common.Enumerations;
using WebSchool.ViewModels.Application;
using WebSchool.Services.Administration;

namespace WebSchool.WebApplication.Controllers.ApiControllers
{
    [Authorize]
    [ApiController]
    [Route("/apiApplications/[action]")]
    public class ApiApplicationsController : Controller
    {
        private readonly IAdministrationService administrationService;
        private readonly IApplicationsService applicationsService;
        private readonly IGroupsService groupsService;

        public ApiApplicationsController(
            IAdministrationService administrationService,
            IApplicationsService applicationsService, 
            IGroupsService groupsService)
        {
            this.administrationService = administrationService;
            this.applicationsService = applicationsService;
            this.groupsService = groupsService;
        }

        [HttpPost]
        public async Task<IActionResult> Approve(ApplicationReviewInputModel input)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(administrationService.ValidateIfUserIsAdmin(userId, input.GroupId) == false)
            {
                return BadRequest();
            }

            if(ModelState.IsValid == false)
            {
                return BadRequest();
            }

            var roleParseResult = Enum.TryParse<GroupRole>(input.Role, true ,out var role);
            if(roleParseResult == false)
            {
                return BadRequest();
            }

            await applicationsService.ApproveAsync(input.ApplicantId, input.GroupId);
            await groupsService.AddUserToGroupAsync(input.ApplicantId, input.GroupId, role);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Decline(ApplicationReviewInputModel input)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (administrationService.ValidateIfUserIsAdmin(userId, input.GroupId) == false)
            {
                return BadRequest();
            }

            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }

            await applicationsService.DeclineAsync(input.ApplicantId, input.GroupId);

            return Ok();
        }
    }
}
