using System.Threading.Tasks;
using WebSchool.Models.Subject;
using System.Collections.Generic;

namespace WebSchool.Services.Contracts
{
    public interface ISubjectService
    {
        bool DoesSubjectExists(string subject, string schoolId);

        Task CreateSubject(string title, string schoolId);

        ICollection<SubjectViewModel> GetSubjects(string schoolId);

        Task Remove(string id);
    }
}
