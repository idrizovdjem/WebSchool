using WebSchool.Data;
using NUnit.Framework;
using WebSchool.Services;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebSchool.Tests
{
    public class StudentsServiceTests
    {
        [Test]
        public async Task GetStudentIdsWithMatchingEmailShouldReturnStudents()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("test");
            var context = new ApplicationDbContext(dbOptions.Options);

            var roleService = new WebSchool.Services.RolesService(context);
            var studentsService = new StudentsService(context, roleService);

            var school = new School();
            await context.Schools.AddAsync(school);
            await context.SaveChangesAsync();

            var students = studentsService.GetStudentIdsWithMatchingEmail("email", "12 D", school.Id);
            Assert.That(students.Count == 0);
        }
    }
}
