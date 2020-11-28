﻿using WebSchool.Data.Models;
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
        private readonly IUsersService usersService;
        private readonly ISchoolService schoolService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UsersController(ILinksService linksService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ISchoolService schoolService, IUsersService usersService)
        {
            this.userManager = userManager;
            this.usersService = usersService;
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
               Email = registerLink.To,
               UserName = registerLink.To
            };

            var result = await this.usersService.CreateUserAsync(user, input.Password);
            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    this.ModelState.AddModelError("Invalid data", error.Description);
                }

                return View(input);
            }    

            await this.usersService.AddUserToRole(user, registerLink.RoleName);
            await this.usersService.Login(user.Email, input.Password);
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

        public IActionResult Login()
        {
            if(this.User.Identity.IsAuthenticated)
            {
                return Redirect("/School/Forum");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserInputModel input)
        {
            if(this.User.Identity.IsAuthenticated)
            {
                return Redirect("/School/Forum");
            }

            if(!this.ModelState.IsValid)
            {
                return View(input);
            }

            var result = await this.usersService.Login(input.Email, input.Password);
            if(!result)
            {
                this.ModelState.AddModelError("Login failed", "Invalid username or password");
                return View(input);
            }

            return Redirect("/School/Forum");
        }
    }
}