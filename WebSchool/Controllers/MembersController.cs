using System;
using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;

using WebSchool.ViewModels.Users;
using WebSchool.Common.Enumerations;
using WebSchool.Services.Administration;
using WebSchool.Services.Common;

namespace WebSchool.WebApplication.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMembersService membersService;
        private readonly IUsersService usersService;

        public MembersController(
            IMembersService membersService, 
            IUsersService usersService)
        {
            this.membersService = membersService;
            this.usersService = usersService;
        }

        public IActionResult Settings(string memberId, string groupId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = usersService.GetRoleInGroup(userId, groupId);
            if (userRole != GroupRole.Admin)
            {
                return Redirect("/Groups/Index");
            }

            var member = membersService.GetById(memberId, groupId);
            if(member == null)
            {
                return Redirect("/Administration?groupId=" + groupId);
            }
            
            return View(member);
        }

        public async Task<IActionResult> SaveChanges(MemberInputModel input)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = usersService.GetRoleInGroup(userId, input.GroupId);
            if(userRole != GroupRole.Admin)
            {
                return Redirect("/Groups/Index");
            }

            if(ModelState.IsValid == false)
            {
                return Redirect("/Groups/Index");
            }

            var isRoleParsed = Enum.TryParse<GroupRole>(input.Role, out var role);
            if(isRoleParsed == false)
            {
                return Redirect("/Groups/Index");
            }

            await membersService.UpdateAsync(input.MemberId, input.GroupId, role);

            return Redirect("/Administration?groupId=" + input.GroupId);
        }

        public async Task<IActionResult> Remove(string memberId, string groupId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = usersService.GetRoleInGroup(userId, groupId);
            if (userRole != GroupRole.Admin)
            {
                return Redirect("/Groups/Index");
            }

            await membersService.RemoveAsync(memberId, groupId);
            return Redirect("/Administration?groupId=" + groupId);
        }
    }
}
