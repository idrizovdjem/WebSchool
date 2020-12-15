using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using WebSchool.ViewModels.School;
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

        public async Task AssignUserToSchool(ApplicationUser user, string schoolId)
        {
            user.SchoolId = schoolId;
            await this.context.SaveChangesAsync();
        }

        public async Task CreateAsync(School school)
        {
            await this.context.Schools.AddAsync(school);
            await this.context.SaveChangesAsync();
        }

        public SchoolViewModel GetSchool(ApplicationUser user, int page)
        {
            var school = this.context.Schools
                .FirstOrDefault(s => s.Id == user.SchoolId);

            if (school == null)
            {
                return null;
            }

            var schoolViewModel = new SchoolViewModel()
            {
                Name = school.Name,
                ImageUrl = school.ImageUrl,
                Page = page,
                Posts = this.postsService.GetPosts(school.Id, page),
                MaxPages = this.postsService.GetMaxPages(school.Id),
            };

            return schoolViewModel;
        }

        public bool IsSchoolNameAvailable(string schoolName)
        {
            return !this.context.Schools.Any(s => s.Name == schoolName);
        }
    }
}
