using System.Threading.Tasks;

namespace WebSchool.Services.Contracts
{
    public interface ILinksService
    {
        Task GenerateLinks(string roleName, int count);
    }
}
