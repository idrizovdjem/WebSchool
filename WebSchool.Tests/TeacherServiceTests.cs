using WebSchool.Data;
using NUnit.Framework;
using WebSchool.Services;
using Microsoft.EntityFrameworkCore;

namespace WebSchool.Tests
{
    public class TeacherServiceTests
    {
        [Test]
        public void GetTeacherShouldReturnNullWhenTeacherIsMissing()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("test");
            var context = new ApplicationDbContext(dbOptions.Options);

            var rolesService = new WebSchool.Services.RolesService(context);
            var linksService = new LinksService(context);
            var usersService = new UsersService(context, rolesService, linksService);
            var classesService = new ClassesService(context, usersService, rolesService);
            var teacherService = new TeacherService(context, classesService, rolesService);

            var result = teacherService.GetTeacher("id");
            Assert.Null(result);
        }

        [Test]
        public void GetTeachersShouldReturnCollectionOfTeachers()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("test");
            var context = new ApplicationDbContext(dbOptions.Options);

            var rolesService = new WebSchool.Services.RolesService(context);
            var linksService = new LinksService(context);
            var usersService = new UsersService(context, rolesService, linksService);
            var classesService = new ClassesService(context, usersService, rolesService);
            var teacherService = new TeacherService(context, classesService, rolesService);

            var result = teacherService.GetTeachers("id");
            Assert.True(result.Count == 0);
        }
    }
}
