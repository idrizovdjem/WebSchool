using WebSchool.Data;
using NUnit.Framework;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebSchool.Tests
{
    public class RolesService
    {
        [Test]
        public void GetUserRoleShouldReturnNullWhenUserIsMissing()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("test");
            var context = new ApplicationDbContext(dbOptions.Options);
            var rolesService = new WebSchool.Services.RolesService(context);

            Assert.Null(rolesService.GetUserRole(""));
        }
    }
}
