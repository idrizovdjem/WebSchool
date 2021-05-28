using System.Linq;
using System.Threading.Tasks;

using WebSchool.Data;
using WebSchool.Data.Models;
using WebSchool.Services.Contracts;
using WebSchool.Common.Enumerations;
using WebSchool.ViewModels.Application;

namespace WebSchool.Services
{
    public class ApplicationsService : IApplicationsService
    {
        private readonly ApplicationDbContext dbContext;

        public ApplicationsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task ApplyAsync(string userId, string groupId)
        {
            var application = new Application()
            {
                UserId = userId,
                GroupId = groupId,
                IsConfirmed = false
            };

            await dbContext.Applications.AddAsync(application);
            await dbContext.SaveChangesAsync();
        }

        public async Task ApproveAsync(string applicantId, string groupId)
        {
            var application = dbContext.Applications
                .FirstOrDefault(a => a.UserId == applicantId && a.GroupId == groupId);

            if (application == null)
            {
                return;
            }

            application.IsConfirmed = true;
            await dbContext.SaveChangesAsync();
        }

        public async Task DeclineAsync(string applicantId, string groupId)
        {
            var application = dbContext.Applications
                .FirstOrDefault(a => a.UserId == applicantId && a.GroupId == groupId);

            if(application == null)
            {
                return;
            }

            dbContext.Applications.Remove(application);
            await dbContext.SaveChangesAsync();
        }

        public ApplicationViewModel[] GetApplications(string groupId)
        {
            return dbContext.Applications
                .Where(a => a.GroupId == groupId && a.IsConfirmed == false)
                .Select(a => new ApplicationViewModel()
                {
                    ApplicantId = a.UserId,
                    Applicant = a.User.Email
                })
                .ToArray();
        }

        public ApplicationStatus GetApplicationStatus(string userId, string groupId)
        {
            var application = dbContext.Applications
                .FirstOrDefault(a => a.UserId == userId && a.GroupId == groupId);

            if (application == null)
            {
                return ApplicationStatus.NotApplied;
            }

            return application.IsConfirmed ? ApplicationStatus.InGroup : ApplicationStatus.WaitingApproval;
        }
    }
}
