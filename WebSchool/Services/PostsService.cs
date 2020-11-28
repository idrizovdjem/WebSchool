using WebSchool.Models.Post;
using System.Collections.Generic;
using WebSchool.Services.Contracts;
using System.Linq;
using WebSchool.Data;

namespace WebSchool.Services
{
    public class PostsService : IPostsService
    {
        private readonly ApplicationDbContext context;

        public PostsService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public ICollection<PostViewModel> GetPosts(string schoolId, int page)
        {
            return this.context.Posts
                .Where(x => x.SchoolId == schoolId)
                .OrderBy(x => x.CreatedOn)
                .Select(x => new PostViewModel()
                {
                    Creator = x.Creator,
                    Content = x.Content,
                    CreatedOn = x.CreatedOn
                })
                .Skip(page * 10)
                .Take(10)
                .ToList();
        }
    }
}
