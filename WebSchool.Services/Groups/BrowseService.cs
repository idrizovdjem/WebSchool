using System.Linq;

using WebSchool.Data;
using WebSchool.Services.Common;
using WebSchool.ViewModels.Group;
using WebSchool.Common.Constants;
using WebSchool.Common.Enumerations;
using WebSchool.Services.Administration;

namespace WebSchool.Services.Groups
{
    public class BrowseService : IBrowseService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IApplicationsService applicationsService;
        private readonly IUsersService usersService;

        public BrowseService(
            ApplicationDbContext dbContext, 
            IApplicationsService applicationsService,
            IUsersService usersService)
        {
            this.dbContext = dbContext;
            this.applicationsService = applicationsService;
            this.usersService = usersService;
        }

        public BrowseGroupViewModel[] GetGroupsContainingName(string userId, string groupName)
        {
            var groups = dbContext.Groups
                .Where(g =>
                    g.Name.Contains(groupName) &&
                    g.IsDeleted == false)
                .Select(g => new BrowseGroupViewModel()
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToArray();

            PopulateGroupsStatus(userId, groups);
            return groups;
        }

        public BrowseGroupViewModel[] GetMostPopular(string userId)
        {
            var groups = dbContext.Groups
                .Where(g => g.Name != GroupConstants.GlobalGroupName)
                .OrderByDescending(g => g.Users.Count)
                .Take(GroupConstants.MostPopularGroupsCount)
                .Select(g => new BrowseGroupViewModel()
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToArray();

            PopulateGroupsStatus(userId, groups);
            return groups;
        }

        private void PopulateGroupsStatus(string userId, BrowseGroupViewModel[] groups)
        {
            foreach (var group in groups)
            {
                if (usersService.IsUserInGroup(userId, group.Id))
                {
                    group.RequestStatus = ApplicationStatus.InGroup.ToString();
                }
                else
                {
                    var applicationStatus = applicationsService.GetApplicationStatus(userId, group.Id);
                    group.RequestStatus = applicationStatus.ToString();
                }
            }
        }
    }
}
