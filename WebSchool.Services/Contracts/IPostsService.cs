using System.Threading.Tasks;
using System.Collections.Generic;

using WebSchool.ViewModels.Post;

namespace WebSchool.Services.Contracts
{
    public interface IPostsService
    {
        IEnumerable<PostViewModel> GetNewestPosts(string groupId, int count = 10);

        Task CreateAsync(CreatePostInputModel input, string userId);
    }
}
