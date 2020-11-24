using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using WebSchool.Models.User;
using System.Threading.Tasks;
using WebSchool.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace WebSchool.Services
{
    public class UserService : IUserService
    {
        private readonly ILinksService linksService;
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public UserService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILinksService linksService)
        {
            this.context = context;
            this.userManager = userManager;
            this.linksService = linksService;
        }

        public async Task RegisterUser(RegisterUserInputModel input)
        {
            var user = new ApplicationUser()
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                Email = input.Email
            };

            // register the user
            await this.userManager.CreateAsync(user, input.Password);
            var createdUser = this.context.Users
                .FirstOrDefault(x => x.Id == user.Id);
            // get the registration link
            var registrationLink = this.linksService.GetLink(input.RegisterLinkId);
            await this.userManager.AddToRoleAsync(createdUser,registrationLink.RoleName);

            await this.context.SaveChangesAsync();
        }
    }
}
