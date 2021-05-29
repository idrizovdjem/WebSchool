using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using WebSchool.ViewModels.Users;
using WebSchool.Services.Contracts;
using System;
using WebSchool.Common.Enumerations;
using System.Security.Claims;

namespace WebSchool.WebApplication.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMembersService membersService;
        private readonly IAdministrationService administrationService;

        public MembersController(IMembersService membersService, IAdministrationService administrationService)
        {
            this.membersService = membersService;
            this.administrationService = administrationService;
        }

        public IActionResult Settings(string memberId, string groupId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (administrationService.ValidateIfUserIsAdmin(userId, groupId) == false)
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
            if(administrationService.ValidateIfUserIsAdmin(userId, input.GroupId) == false)
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
            if (administrationService.ValidateIfUserIsAdmin(userId, groupId) == false)
            {
                return Redirect("/Groups/Index");
            }

            await membersService.RemoveAsync(memberId, groupId);
            return Redirect("/Administration?groupId=" + groupId);
        }
    }
}
