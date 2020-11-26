using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using WebSchool.Services.Contracts;

namespace WebSchool.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly ApplicationDbContext context;

        public SchoolService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AssignUserToSchool(string userId, string schoolId)
        {
            if(this.context.UserSchools.Any(x => x.UserId == userId && x.SchoolId == schoolId))
            {
                return;
            }

            var userSchool = new UserSchool()
            {
                UserId = userId,
                SchoolId = schoolId
            };

            await this.context.UserSchools.AddAsync(userSchool);
            await this.context.SaveChangesAsync();
        }

        public async Task CreateAsync(School school)
        {
            await this.context.Schools.AddAsync(school);
            await this.context.SaveChangesAsync();
        }
    }
}
