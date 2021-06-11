using System;
using System.Threading.Tasks;

using WebSchool.Data;
using WebSchool.Data.Models;

namespace WebSchool.Services.Common
{
    public class NotificationsService : INotificationsService
    {
        private readonly ApplicationDbContext dbContext;

        public NotificationsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(string userId, string message)
        {
            var notification = new Notification()
            {
                UserId = userId,
                Message = message,
                CreatedOn = DateTime.UtcNow,
                IsActive = true
            };

            await dbContext.Notifications.AddAsync(notification);
            await dbContext.SaveChangesAsync();
        }
    }
}
