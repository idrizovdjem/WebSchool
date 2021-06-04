using System.Linq;

using WebSchool.Data;
using WebSchool.ViewModels.Group;
using WebSchool.Services.Contracts;
using WebSchool.Common.Enumerations;

namespace WebSchool.Services
{
    public class AdministrationService : IAdministrationService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IApplicationsService applicationsService;
        private readonly IMembersService membersService;
        private readonly IUsersService usersService;
        private readonly IPostsService postsService;

        public AdministrationService(
            ApplicationDbContext dbContext, 
            IUsersService usersService, 
            IApplicationsService applicationsService, 
            IMembersService membersService,
            IPostsService postsService)
        {
            this.dbContext = dbContext;
            this.usersService = usersService;
            this.applicationsService = applicationsService;
            this.membersService = membersService;
            this.postsService = postsService;
        }

        public bool ValidateIfUserIsAdmin(string userId, string groupId)
        {
            if (usersService.IsUserInGroup(userId, groupId) == false)
            {
                return false;
            }

            var userRole = usersService.GetRoleInGroup(userId, groupId);
            if (userRole != GroupRole.Admin)
            {
                return false;
            }

            return true;
        }

        public GroupSettingsViewModel GetGroupSettings(string groupId)
        {
            return dbContext.Groups
                .Where(g => g.Id == groupId)
                .Select(g => new GroupSettingsViewModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    MembersCount = membersService.GetCount(g.Id),
                    ActiveApplicationsCount = applicationsService.GetCount(g.Id),
                    PostsCount = postsService.GetCount(g.Id)
                })
                .FirstOrDefault();
        }
    }
}
