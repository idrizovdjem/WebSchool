using WebSchool.Models.User;
using System.Collections.Generic;
using WebSchool.Areas.Admin.Models.User;

namespace WebSchool.Services.Contracts
{
    public interface ITeacherService
    {
        TeacherViewModel GetTeacher(string id);

        ICollection<UsersViewModel> GetTeachers(string schoolId);
    }
}
