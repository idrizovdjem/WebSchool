using System.Threading.Tasks;

namespace WebSchool.Services.Contracts
{
    public interface ISubjectService
    {
        bool DoesSubjectExists(string subject, string schoolId);

        Task CreateSubject(string title, string schoolId);
    }
}
