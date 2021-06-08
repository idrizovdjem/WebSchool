using System.Linq;

using WebSchool.Data;
using WebSchool.Services.Posts;
using WebSchool.Services.Common;
using WebSchool.ViewModels.Group;

namespace WebSchool.Services.Administration
{
    public class AdministrationService : IAdministrationService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IApplicationsService applicationsService;
        private readonly IMembersService membersService;
        private readonly IPostsService postsService;

        public AdministrationService(
            ApplicationDbContext dbContext, 
            IApplicationsService applicationsService, 
            IMembersService membersService,
            IPostsService postsService)
        {
            this.dbContext = dbContext;
            this.applicationsService = applicationsService;
            this.membersService = membersService;
            this.postsService = postsService;
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
