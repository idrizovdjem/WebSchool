using System.Threading.Tasks;

namespace WebSchool.Services.Contracts
{
    public interface IUsersService
    {
        Task<bool> Login(string email, string password);
    }
}
