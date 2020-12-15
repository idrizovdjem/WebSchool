using System.Linq;
using WebSchool.Data;
using NUnit.Framework;
using WebSchool.Services;
using WebSchool.Data.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebSchool.Tests
{
    public class PostsServiceTests
    {
        [Test]
        public async Task CreatePostShouldSuccessfullyCreatePost()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("test");
            var context = new ApplicationDbContext(dbOptions.Options);

            var postsService = new PostsService(context, null);

            var school = new School();
            var user = new ApplicationUser();
            await context.Schools.AddAsync(school);
            await context.Users.AddAsync(user);

            await postsService.CreatePost("Content", user, school.Id);
            var postsCount = context.Posts.Count();
            Assert.That(postsCount != 0);
        }

        [Test]
        public async Task GetPostsShouldReturnPosts()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("test");
            var context = new ApplicationDbContext(dbOptions.Options);

            var postsService = new PostsService(context, null);

            var school = new School();
            var user = new ApplicationUser();
            await context.Schools.AddAsync(school);
            await context.Users.AddAsync(user);

            await postsService.CreatePost("Content", user, school.Id);
            var posts = postsService.GetPosts(school.Id, 0);
            Assert.That(posts.Count > 0);
        }

        [Test]
        public async Task GetPostShouldReturnTheCorrectPost()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("test");
            var context = new ApplicationDbContext(dbOptions.Options);

            var postsService = new PostsService(context, null);

            var school = new School();
            var user = new ApplicationUser();
            await context.Schools.AddAsync(school);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            await postsService.CreatePost("Content", user, school.Id);
            var post = context.Posts.FirstOrDefault();
            var posts = postsService.GetPost(post.Id);
            Assert.That(posts != null);
        }

        [Test]
        public async Task GetMaxPagesShouldReturnCorrectNumber()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("test");
            var context = new ApplicationDbContext(dbOptions.Options);

            var postsService = new PostsService(context, null);

            var school = new School();
            var user = new ApplicationUser();
            await context.Schools.AddAsync(school);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            await postsService.CreatePost("Content", user, school.Id);
            var maxPages = postsService.GetMaxPages(school.Id);
            Assert.That(maxPages == 1);
        }
    }
}
