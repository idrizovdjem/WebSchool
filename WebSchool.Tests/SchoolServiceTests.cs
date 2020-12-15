using System.Linq;
using WebSchool.Data;
using NUnit.Framework;
using WebSchool.Services;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebSchool.Tests
{
    public class SchoolServiceTests
    {
        [Test]
        public async Task AddSchoolShouldAddCorrectlySchool()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("test");
            var context = new ApplicationDbContext(dbOptions.Options);

            var school = new School();
            var schoolService = new SchoolService(context, null);
            await schoolService.CreateAsync(school);

            Assert.That(context.Schools.Count() == 1);
        }

        [Test]
        public void IsSchoolNameAvailableShouldReturnTrueIfSchoolNameIsAvailabe()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("test");
            var context = new ApplicationDbContext(dbOptions.Options);

            var schoolService = new SchoolService(context, null);
            var result = schoolService.IsSchoolNameAvailable("available");
            Assert.True(result);
        }

        [Test]
        public async Task IsSchoolNameAvailableShouldReturnFalseIfNameIsNotAvailable()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("test");
            var context = new ApplicationDbContext(dbOptions.Options);

            var school = new School()
            {
                Name = "taken"
            };
            await context.Schools.AddAsync(school);
            await context.SaveChangesAsync();

            var schoolService = new SchoolService(context, null);
            var result = schoolService.IsSchoolNameAvailable("taken");
            Assert.False(result);
        }

        [Test]
        public void GetSchoolShouldReturnNullIfUserDoesNotExists()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("test");
            var context = new ApplicationDbContext(dbOptions.Options);
            var schoolService = new SchoolService(context, null);
            var user = new ApplicationUser();
            var result = schoolService.GetSchool(user, 0);
            Assert.Null(result);
        }

        [Test]
        public async Task GetSchoolShouldReturnSchoolViewModel()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("test");
            var context = new ApplicationDbContext(dbOptions.Options);
            var commentsService = new CommentsService(context);
            var postsService = new PostsService(context, commentsService);

            var schoolService = new SchoolService(context, postsService);

            var school = new School()
            {
                Name = "test"
            };

            var user = new ApplicationUser()
            {
                SchoolId = school.Id
            };

            await context.Schools.AddAsync(school);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var result = schoolService.GetSchool(user, 0);
            Assert.That(result != null);
        }

        [Test]
        public async Task AssignUserToSchoolShouldAddTheUserToTheSchool()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("test");
            var context = new ApplicationDbContext(dbOptions.Options);
            var schoolService = new SchoolService(context, null);

            var school = new School();
            var user = new ApplicationUser();
            await context.Schools.AddAsync(school);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            await schoolService.AssignUserToSchool(user, school.Id);

            var updatedUser = context.Users
                .FirstOrDefault(x => x.Id == user.Id);
            Assert.False(string.IsNullOrWhiteSpace(updatedUser.SchoolId));
        }
    }
}
