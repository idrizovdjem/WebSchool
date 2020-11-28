using WebSchool.Data.Models;
using System.Threading.Tasks;
using WebSchool.Models.School;

namespace WebSchool.Services.Contracts
{
    public interface ISchoolService
    {
        Task CreateAsync(School school);

        Task AssignUserToSchool(string userId, string schoolId);

        bool IsSchoolNameAvailable(string schoolName);

        SchoolViewModel GetSchool(ApplicationUser user, int page);
    }
}
