using System.Linq;
using WebSchool.Data;
using WebSchool.Models.User;
using WebSchool.Services.Contracts;

namespace WebSchool.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext context;
        private readonly IClassesService classesService;

        public TeacherService(ApplicationDbContext context, IClassesService classesService)
        {
            this.context = context;
            this.classesService = classesService;
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
                    Classes = this.classesService.GetUserClasses(x.Id)
                })
                .FirstOrDefault();
        }
    }
}
