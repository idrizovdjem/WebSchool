using WebSchool.Models.User;

namespace WebSchool.Services.Contracts
{
    public interface ITeacherService
    {
        TeacherViewModel GetTeacher(string id);
    }
}
