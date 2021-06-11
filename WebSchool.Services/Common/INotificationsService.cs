using System.Threading.Tasks;

namespace WebSchool.Services.Common
{
    public interface INotificationsService
    {
        Task CreateAsync(string userId, string message);
    }
}
