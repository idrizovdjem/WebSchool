using System.Threading.Tasks;

using WebSchool.ViewModels.Post;

namespace WebSchool.Services.Contracts
{
    public interface IPostsService
    {
        PostViewModel[] GetNewestPosts(string userId, string groupId, int count = 10);

        Task CreateAsync(CreatePostInputModel input, string userId);

        PostViewModel GetById(string userId, string postId);

        PostViewModel[] GetAll(string groupId);
    }
}
