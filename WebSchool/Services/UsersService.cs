using WebSchool.Data;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using WebSchool.Services.Contracts;
using Microsoft.AspNetCore.Identity;

namespace WebSchool.Services
{
    public class UsersService : IUsersService
    {
        private readonly ILinksService linksService;
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILinksService linksService)
        {
            this.context = context;
            this.userManager = userManager;
            this.linksService = linksService;
        }

        public async Task RegisterUserAsync(ApplicationUser user)
        {
            await this.context.Users.AddAsync(user);
            await this.context.SaveChangesAsync();
        }
    }
}
