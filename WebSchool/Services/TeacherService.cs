using System.Linq;
using WebSchool.Data;
using WebSchool.Models.User;
using System.Collections.Generic;
using WebSchool.Services.Contracts;
using WebSchool.Areas.Admin.Models.User;

namespace WebSchool.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext context;
        private readonly IClassesService classesService;
        private readonly IRolesService rolesService;

        public TeacherService(ApplicationDbContext context, IClassesService classesService, IRolesService rolesService)
        {
            this.context = context;
            this.classesService = classesService;
            this.rolesService = rolesService;
        }

        public TeacherViewModel GetTeacher(string id)
        {
            return this.context.Users
                .Where(x => x.Id == id)
                .Select(x => new TeacherViewModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Classes = this.classesService.GetTeacherAssignedClasses(x.Id)
                })
                .FirstOrDefault();
        }

        public ICollection<UsersViewModel> GetTeachers(string schoolId)
        {
            return this.context.Users
                .Where(x => x.SchoolId == schoolId)
                .Select(x => new UsersViewModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Role = this.rolesService.GetUserRole(x.Id)
                })
                .ToList()
                .Where(x => x.Role == "Teacher")
                .ToList();
        }
    }
}
