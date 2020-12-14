using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using WebSchool.Data;
using WebSchool.Services;

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
    }
}
