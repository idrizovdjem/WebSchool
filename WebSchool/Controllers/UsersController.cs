using WebSchool.Data.Models;
using WebSchool.Models.User;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSchool.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace WebSchool.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILinksService linksService;
        private readonly IUsersService usersService;
        private readonly IRolesService rolesService;
        private readonly IStudentsService studentsService;
        private readonly ISchoolService schoolService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UsersController(ILinksService linksService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ISchoolService schoolService, IUsersService usersService, IRolesService rolesService, IStudentsService studentsService)
        {
            this.userManager = userManager;
            this.usersService = usersService;
            this.rolesService = rolesService;
            this.studentsService = studentsService;
            this.linksService = linksService;
            this.signInManager = signInManager;
            this.schoolService = schoolService;
        }

        public IActionResult Register()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return Redirect("/School/Forum");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserInputModel input)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return Redirect("/School/Forum");
            }

            if (!this.ModelState.IsValid)
            {
                return View(input);
            }

            var registerLink = this.linksService.GetLink(input.RegistrationLinkId);
            if (registerLink == null)
            {
                return NotFound();
            }

            if (input.Password != input.ConfirmPassword)
            {
                this.ModelState.AddModelError("Password", "Passwords does not match");
                return View(input);
            }

            var user = new ApplicationUser()
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                Email = registerLink.To,
                UserName = registerLink.To
            };

            var result = await this.userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError("Invalid data", error.Description);
                }

                return View(input);
            }

            await this.rolesService.AddUserToRole(user, registerLink.RoleName);
            await this.usersService.Login(user.Email, input.Password);
            await this.linksService.UseLink(input.RegistrationLinkId);

            if (registerLink.RoleName == "Admin")
            {
                return Redirect("/School/Create");
            }

            if (string.IsNullOrWhiteSpace(registerLink.SchoolId))
            {
                return BadRequest();
            }

            await this.schoolService.AssignUserToSchool(user, registerLink.SchoolId);
            return Redirect("/School/Forum");
        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return Redirect("/School/Forum");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserInputModel input)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return Redirect("/School/Forum");
            }

            if (!this.ModelState.IsValid)
            {
                return View(input);
            }

            var result = await this.usersService.Login(input.Email, input.Password);
            if (!result)
            {
                this.ModelState.AddModelError("Login failed", "Invalid username or password");
                return View(input);
            }

            return Redirect("/School/Forum");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetStudentsWithEmail(string email, string signature)
        {
            var schoolId = await this.schoolService.GetSchoolId(this.User);
            var userEmails = this.studentsService.GetStudentIdsWithMatchingEmail(email, signature, schoolId);
            return Json(userEmails);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditUser(string id)
        {
            var user = this.usersService.GetUserForEdit(id);
            if (user == null)
            {
                return Redirect("/Admin/Administration/Users");
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(UsersViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return View();
            }

            await this.usersService.UpdateUser(input);

            return Redirect("/Admin/Administration/Users");
        }
    }
}