using System;
using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using WebSchool.Models.Post;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebSchool.Services.Contracts;

namespace WebSchool.Services
{
    public class PostsService : IPostsService
    {
        private readonly ApplicationDbContext context;

        public PostsService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task CreatePost(CreatePostInputModel input, ApplicationUser user, string schoolId)
        {
            var post = new Post()
            {
                Content = input.Content,
                CreatedOn = DateTime.UtcNow,
                SchoolId = schoolId,
                Creator = user
            };

            await this.context.Posts.AddAsync(post);
            await this.context.SaveChangesAsync();
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
                .Skip((page - 1) * 10)
                .Take(10)
                .ToList();
        }
    }
}
