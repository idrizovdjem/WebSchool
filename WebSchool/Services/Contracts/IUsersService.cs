using WebSchool.Data.Models;
using System.Threading.Tasks;

namespace WebSchool.Services.Contracts
{
    public interface IUsersService
    {
        Task RegisterUserAsync(ApplicationUser input);
    }
}
