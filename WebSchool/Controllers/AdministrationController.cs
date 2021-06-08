using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.Services.Posts;
using WebSchool.Services.Groups;
using WebSchool.ViewModels.Group;
using WebSchool.Services.Common;
using WebSchool.Common.Enumerations;
using WebSchool.Services.Administration;

namespace WebSchool.WebApplication.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
        private readonly IGroupsService groupsService;
        private readonly IApplicationsService applicationsService;
        private readonly IAdministrationService administrationService;
        private readonly IPostsService postsService;
        private readonly IMembersService membersService;
        private readonly IUsersService usersService;

        public AdministrationController(
            IPostsService postsService, 
            IGroupsService groupsService, 
            IMembersService membersService,
            IApplicationsService applicationsService,
            IAdministrationService administrationService)
        {
            this.groupsService = groupsService;
            this.applicationsService = applicationsService;
            this.administrationService = administrationService;
            this.postsService = postsService;
            this.membersService = membersService;
        }

        public IActionResult Index(string groupId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = usersService.GetRoleInGroup(userId, groupId);
            if (userRole != GroupRole.Admin)
            {
                return Redirect("/Groups/Index");
            }

            var viewModel = administrationService.GetGroupSettings(groupId);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeGroupName(ChangeGroupNameInputModel input)
        {
            if(ModelState.IsValid == false)
            {
                return Redirect("/Administration/Index?groupId=" + input.Id);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = usersService.GetRoleInGroup(userId, input.Id);
            if (userRole != GroupRole.Admin)
            {
                return Redirect("/Groups/Index");
            }

            if (groupsService.IsGroupNameAvailable(input.Name) == false)
            {
                return Redirect("/Administration/Index?groupId=" + input.Id);
            }

            await groupsService.ChangeNameAsync(input);
            return Redirect("/Administration/Index?groupId=" + input.Id);
        }

        public IActionResult Applications(string groupId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = usersService.GetRoleInGroup(userId, groupId);
            if (userRole != GroupRole.Admin)
            {
                return Redirect("/Groups/Index");
            }

            ViewData["GroupId"] = groupId;
            var applications = applicationsService.GetApplications(groupId);
            return View(applications);
        }

        public IActionResult Members(string groupId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = usersService.GetRoleInGroup(userId, groupId);
            if (userRole != GroupRole.Admin)
            {
                return Redirect("/Groups/Index");
            }

            ViewData["GroupId"] = groupId;

            var groupMembers = membersService.GetMembers(userId, groupId);
            return View(groupMembers);
        }

        public IActionResult Posts(string groupId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = usersService.GetRoleInGroup(userId, groupId);
            if (userRole != GroupRole.Admin)
            {
                return Redirect("/Groups/Index");
            }

            var groupPosts = postsService.GetAll(groupId);
            return View(groupPosts);
        }
    }
}
