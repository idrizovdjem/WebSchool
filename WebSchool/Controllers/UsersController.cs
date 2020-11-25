using System;
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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UsersController(ILinksService linksService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.linksService = linksService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserInputModel input)
        {
            if(!this.ModelState.IsValid)
            {
                throw new ArgumentException("Invalid user information");
            }

            var registerLink = this.linksService.GetLink(input.RegisterLinkId);
            if(registerLink == null)
            {
                throw new ArgumentException("Invalid registration link");
            }

            if(input.Password != input.ConfirmPassword)
            {
                throw new ArgumentException("Passwords doesn't match!");
            }

            var user = new ApplicationUser()
            {
               FirstName = input.FirstName,
               LastName = input.LastName,
               Email = input.Email,
            };

            var createdUser = await this.userManager.CreateAsync(user, input.Password);
            await this.userManager.AddToRoleAsync(user, registerLink.RoleName);

            if(registerLink.RoleName == "Admin")
            {
                // go and create new school
                await this.signInManager.SignInAsync(user, false);
                return Redirect("/School/Create");
            }

            // assign the user to the school
            // implement later
            return Redirect("/School/Forum");
        }
    }
}