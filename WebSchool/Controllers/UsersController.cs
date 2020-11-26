using WebSchool.Data.Models;
using WebSchool.Models.User;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSchool.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace WebSchool.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILinksService linksService;
        private readonly ISchoolService schoolService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UsersController(ILinksService linksService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ISchoolService schoolService)
        {
            this.userManager = userManager;
            this.linksService = linksService;
            this.signInManager = signInManager;
            this.schoolService = schoolService;
        }

        public IActionResult Register()
        {
            if(this.User.Identity.IsAuthenticated)
            {
                return Redirect("/School/Forum");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserInputModel input)
        {
            if(this.User.Identity.IsAuthenticated)
            {
                return Redirect("/School/Forum");
            }

            if(!this.ModelState.IsValid)
            {
                return View(input);
            }

            var registerLink = this.linksService.GetLink(input.RegistrationLinkId);
            if(registerLink == null)
            {
                return NotFound();
            }

            if(input.Password != input.ConfirmPassword)
            {
                this.ModelState.AddModelError("Password", "Passwords does not match");
                return View(input);
            }

            var user = new ApplicationUser()
            {
               FirstName = input.FirstName,
               LastName = input.LastName,
               Email = input.Email,
               UserName = input.Email
            };

            var result = await this.userManager.CreateAsync(user, input.Password);
            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    this.ModelState.AddModelError("Invalid data", error.Description);
                }

                return View(input);
            }    

            await this.userManager.AddToRoleAsync(user, registerLink.RoleName);
            await this.signInManager.PasswordSignInAsync(input.Email, input.Password, false, false);

            await this.linksService.UseLink(input.RegistrationLinkId);

            if (registerLink.RoleName == "Admin")
            {
                return Redirect("/School/Create");
            }

            if(string.IsNullOrWhiteSpace(registerLink.SchoolId))
            {
                return BadRequest();
            }

            await this.schoolService.AssignUserToSchool(user.Id, registerLink.SchoolId);
            return Redirect("/School/Forum");
        }
    }
}