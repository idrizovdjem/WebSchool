using WebSchool.Data.Models;
using System.Threading.Tasks;

namespace WebSchool.Services.Contracts
{
    public interface ISchoolService
    {
        Task CreateAsync(School school);

        Task AssignUserToSchool(string userId, string schoolId);

        bool IsSchoolNameAvailable(string schoolName);
    }
}
