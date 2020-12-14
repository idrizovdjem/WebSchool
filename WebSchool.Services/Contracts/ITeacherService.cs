using WebSchool.ViewModels.User;
using System.Collections.Generic;

namespace WebSchool.Services.Contracts
{
    public interface ITeacherService
    {
        TeacherViewModel GetTeacher(string id);

        ICollection<UsersViewModel> GetTeachers(string schoolId);
    }
}
