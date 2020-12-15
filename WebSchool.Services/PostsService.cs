using System;
using System.Linq;
using WebSchool.Data;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using WebSchool.ViewModels.Post;
using System.Collections.Generic;
using WebSchool.Services.Contracts;

namespace WebSchool.Services
{
    public class PostsService : IPostsService
    {
        private readonly ApplicationDbContext context;
        private readonly ICommentsService commentsService;

        public PostsService(ApplicationDbContext context, ICommentsService commentsService)
        {
            this.context = context;
            this.commentsService = commentsService;
        }

        public async Task CreatePost(string content, ApplicationUser user, string schoolId)
        {
            var post = new Post()
            {
                Content = content,
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
                .Where(x => x.SchoolId == schoolId && x.IsDeleted == false)
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new PostViewModel()
                {
                    Id = x.Id,
                    Creator = x.Creator.Email,
                    Content = x.Content,
                    CreatedOn = x.CreatedOn,
                    Comments = this.commentsService.GetComments(x.Id),
                })
                .Skip((page - 1) * 10)
                .Take(10)
                .ToList();
        }

        public Post GetPost(string postId)
        {
            return this.context.Posts
                .FirstOrDefault(x => x.Id == postId && x.IsDeleted == false);
        }

        public int GetMaxPages(string schoolId)
        {
            var postsCount = this.context.Posts.
                Count(x => x.SchoolId == schoolId);

            var maxPages = postsCount / 10;
            if (maxPages % 10 > 0 || postsCount < 10)
            {
                maxPages++;
            }

            return maxPages;
        }
    }
}
