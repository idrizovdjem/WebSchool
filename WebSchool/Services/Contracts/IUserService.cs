using WebSchool.Models.User;
using System.Threading.Tasks;

namespace WebSchool.Services.Contracts
{
    public interface IUserService
    {
        Task RegisterUser(RegisterUserInputModel input);
    }
}
