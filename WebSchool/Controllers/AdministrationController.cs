﻿using System.Threading.Tasks;
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
        private readonly IApplicationsService applicationsService;
        private readonly IAdministrationService administrationService;
        private readonly IPostsService postsService;
        private readonly IMembersService membersService;

        public AdministrationController(IGroupsService groupsService, IApplicationsService applicationsService, IAdministrationService administrationService, IPostsService postsService, IMembersService membersService)
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
            if (administrationService.ValidateIfUserIsAdmin(userId, groupId) == false)
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

        public IActionResult Members(string groupId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(administrationService.ValidateIfUserIsAdmin(userId, groupId) == false)
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
            if (administrationService.ValidateIfUserIsAdmin(userId, groupId) == false)
            {
                return Redirect("/Groups/Index");
            }

            var groupPosts = postsService.GetAll(groupId);
            return View(groupPosts);
        }
    }
}
