using WebSchool.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace WebSchool.Data.Seeders
{
    public class RoleSeeder
    {
        private readonly RoleManager<ApplicationRole> roleManager;

        public RoleSeeder(RoleManager<ApplicationRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public void Seed()
        {
            var adminRole = new ApplicationRole()
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

            var teacherRole = new ApplicationRole()
            {
                Name = "Teacher",
                NormalizedName = "TEACHER"
            };

            var studentRole = new ApplicationRole()
            {
                Name = "Student",
                NormalizedName = "STUDENT"
            };

            roleManager.CreateAsync(adminRole).GetAwaiter().GetResult();
            roleManager.CreateAsync(teacherRole).GetAwaiter().GetResult();
            roleManager.CreateAsync(studentRole).GetAwaiter().GetResult();
        }
    }
}
