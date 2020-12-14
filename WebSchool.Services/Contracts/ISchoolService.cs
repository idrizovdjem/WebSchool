using WebSchool.Data.Models;
using System.Threading.Tasks;
using WebSchool.ViewModels.School;

namespace WebSchool.Services.Contracts
{
    public interface ISchoolService
    {
        Task CreateAsync(School school);

        Task AssignUserToSchool(ApplicationUser user, string schoolId);

        bool IsSchoolNameAvailable(string schoolName);

        SchoolViewModel GetSchool(ApplicationUser user, int page);
    }
}
