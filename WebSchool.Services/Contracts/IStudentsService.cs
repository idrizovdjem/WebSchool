using System.Collections.Generic;

namespace WebSchool.Services.Contracts
{
    public interface IStudentsService
    {
        List<string> GetStudentIdsWithMatchingEmail(string email, string signature, string schoolId);
    }
}
