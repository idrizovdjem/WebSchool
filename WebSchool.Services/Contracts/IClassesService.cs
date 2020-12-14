using System.Threading.Tasks;
using WebSchool.ViewModels.Classes;
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

        bool ClassExists(string signature, string schoolId);

        Task AssignUserToClass(string userId, string signature, string schoolId);

        ICollection<string> GetStudentsFromClass(string signature, string schoolId);
    }
}
