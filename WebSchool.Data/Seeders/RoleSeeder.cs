using Microsoft.AspNetCore.Identity;

using WebSchool.Data.Models;

namespace WebSchool.Data.Seeders
{
    public class RoleSeeder : ISeeder
    {
        private readonly RoleManager<ApplicationRole> roleManager;

        public RoleSeeder(RoleManager<ApplicationRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public void Seed()
        {
            var roleNames = new string[]
            {
                "Admin", "Teacher", "Student"
            };

            foreach(var roleName in roleNames)
            {
                var role = new ApplicationRole()
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                };

                roleManager.CreateAsync(role).GetAwaiter().GetResult();
            }
        }
    }
}
