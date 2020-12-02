using System.Threading.Tasks;
using WebSchool.Models.Classes;
using System.Collections.Generic;

namespace WebSchool.Services.Contracts
{
    public interface IClassesService
    {
        bool IsClassSignatureAvailable(string signature, string schoolId);

        Task CreateClass(string signature, string schoolId);

        ICollection<ClassViewModel> GetClasses(string schoolId);

        SchoolClassViewModel GetClassInformation(string signature, string schoolId);

        Task AddStudentsToClass(string signature, List<string> emails, string schoolId);

        Task Remove(string signature, string email, string schoolId);

        Task RemoveClassFromUser(string classId, string userId);

        ICollection<TeacherClassViewModel> GetTeacherAssignedClasses(string teacherId);

        ICollection<TeacherClassViewModel> GetClassesWithoutTeacher(string teacherId, string schoolId);
    }
}
