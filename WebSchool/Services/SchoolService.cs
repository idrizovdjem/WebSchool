using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using WebSchool.Models.School;
using WebSchool.Services.Contracts;

namespace WebSchool.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly ApplicationDbContext context;
        private readonly IPostsService postsService;

        public SchoolService(ApplicationDbContext context, IPostsService postsService)
        {
            this.context = context;
            this.postsService = postsService;
        }

        public async Task AssignUserToSchool(string userId, string schoolId)
        {
            if (this.context.UserSchools.Any(x => x.UserId == userId && x.SchoolId == schoolId))
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


        public SchoolViewModel GetSchool(ApplicationUser user, int page)
        {
            var schoolId = this.GetSchoolIdByUser(user);

            var school = this.context.Schools
                .FirstOrDefault(s => s.Id == schoolId);

            var schoolViewModel = new SchoolViewModel()
            {
                Name = school.Name,
                ImageUrl = school.ImageUrl,
                Page = page,
                Posts = this.postsService.GetPosts(schoolId, page),
                MaxPages = this.GetMaxPage(schoolId),
            };

            return schoolViewModel;
        }

        public string GetSchoolIdByUser(ApplicationUser user)
        {
            return this.context.UserSchools
                .Where(x => x.UserId == user.Id)
                .Select(x => x.SchoolId)
                .FirstOrDefault();
        }

        public bool IsSchoolNameAvailable(string schoolName)
        {
            return !this.context.Schools.Any(s => s.Name == schoolName);
        }

        private int GetMaxPage(string schoolId)
        {
            var postsCount = this.context.Schools
                .FirstOrDefault(s => s.Id == schoolId)
                .Posts.Count();

            var maxPages = postsCount / 10;
            if (postsCount % 10 > 0)
            {
                maxPages++;
            }

            return maxPages;
        }
    }
}
