using WebSchool.Data;
using NUnit.Framework;
using WebSchool.Services;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebSchool.Tests
{
    public class UsersServiceTests
    {
        [Test]
        public void GetUserByEmailReturnsNullWhenThereIsNoUserWithThatEmail()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");
            var dbContext = new ApplicationDbContext(dbOptions.Options);

            var usersService = new UsersService(dbContext, null, null);
            var result = usersService.GetUserByEmail("test@mail.com");

            Assert.Null(result);
        }

        [Test]
        public async Task GetUserByEmailReturnsUser()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");
            var dbContext = new ApplicationDbContext(dbOptions.Options);

            var user = new ApplicationUser()
            {
                Email = "test@mail.com"
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var usersService = new UsersService(dbContext, null, null);
            var result = usersService.GetUserByEmail("test@mail.com");

            Assert.That(result.Id != null);
        }

        [Test]
        public void GetUserByIdReturnsNullWhenThereIsNoUserWithThatId()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("test");
            var dbContext = new ApplicationDbContext(dbOptions.Options);

            var usersService = new UsersService(dbContext, null, null);
            var result = usersService.GetUserById("some id");

            Assert.Null(result);
        }

        [Test]
        public async Task GetUserByIdReturnsUserCorrectly()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("test");
            var context = new ApplicationDbContext(dbOptions.Options);

            var user = new ApplicationUser();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var usersService = new UsersService(context, null, null);
            var result = usersService.GetUserById(user.Id);

            Assert.That(result.Id != null);
        }

        [Test]
        public void GetUserForEditReturnsNullWhenUserWithIdIsMissing()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("test");
            var dbContext = new ApplicationDbContext(dbOptions.Options);

            var usersService = new UsersService(dbContext, null, null);
            var result = usersService.GetUserForEdit("some id");

            Assert.Null(result);
        }

        [Test]
        public async Task GetUserForEditReturnsUserWhenThereisUserWithThatId()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("test");
            var context = new ApplicationDbContext(dbOptions.Options);

            var user = new ApplicationUser();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var usersService = new UsersService(context, null, null);
            var result = usersService.GetUserForEdit(user.Id);

            Assert.That(result.Id != null);
        }
    }
}
