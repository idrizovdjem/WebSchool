using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebSchool.ViewModels.Group;
using WebSchool.Services.Contracts;

namespace WebSchool.Web.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class GroupsController : Controller
    {
        private readonly IGroupsService groupsService;

        public GroupsController(IGroupsService groupsService)
        {
            this.groupsService = groupsService;
        }

        public IActionResult Index(string groupName = "Global Group")
        {
            var groupViewModel = groupsService.GetGroupContent(groupName);
            if(groupViewModel == null)
            {
                return Redirect("/Groups/Index");
            }

            return View(groupViewModel);
        }        

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateGroupInputModel input)
        {
            if(ModelState.IsValid == false)
            {
                return View(input);
            }

            if(groupsService.IsGroupNameAvailable(input.Name) == false)
            {
                ModelState.AddModelError("Name", "This group name is already taken");
                return View(input);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var createGroup = await groupsService.CreateAsync(userId, input.Name);

            return Redirect($"/Groups/Index/groupName={createGroup.Name}");
        }
    }
}
