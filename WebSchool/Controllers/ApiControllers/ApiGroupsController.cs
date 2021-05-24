﻿using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;

using WebSchool.Services.Contracts;

namespace WebSchool.WebApplication.Controllers.ApiControllers
{
    public class ApiGroupsController : Controller
    {
        private readonly IGroupsService groupsService;

        public ApiGroupsController(IGroupsService groupsService)
        {
            this.groupsService = groupsService;
        }

        public IActionResult GetUserGroups()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var groupNames = groupsService.GetUserGroups(userId);
            return Json(groupNames);
        }

        public IActionResult IsNameAvailable(string groupName)
        {
            var isNameAvailable = groupsService.IsGroupNameAvailable(groupName);
            return Json(isNameAvailable);
        }
    }
}
