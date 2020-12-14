using System.Linq;
using WebSchool.Data;
using System.Collections.Generic;
using WebSchool.Services.Contracts;

namespace WebSchool.Services
{
    public class StudentsService : IStudentsService
    {
        private readonly ApplicationDbContext context;
        private readonly IRolesService rolesService;

        public StudentsService(ApplicationDbContext context, IRolesService rolesService)
        {
            this.context = context;
            this.rolesService = rolesService;
        }

        public List<string> GetStudentIdsWithMatchingEmail(string email, string signature, string schoolId)
        {
            var users = this.context.Users
                .Where(x => x.SchoolId == schoolId && x.Email.Contains(email))
                .ToList();

            var schoolClass = this.context.SchoolClasses
                .FirstOrDefault(x => x.SchoolId == schoolId && x.Signature == x.Signature);

            var filteredUsers = new List<string>();
            foreach (var user in users)
            {
                if (this.context.UserClasses.Any(x => x.UserId == user.Id))
                {
                    continue;
                }

                if (this.rolesService.GetUserRole(user.Id) != "Student")
                {
                    continue;
                }

                filteredUsers.Add(user.Email);
            }

            return filteredUsers;
        }
    }
}
