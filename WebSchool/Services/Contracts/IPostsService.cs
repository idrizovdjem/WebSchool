using WebSchool.Data.Models;
using WebSchool.Models.Post;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebSchool.Services.Contracts
{
    public interface IPostsService
    {
        ICollection<PostViewModel> GetPosts(string schoolId, int page);

        Task CreatePost(CreatePostInputModel input, ApplicationUser user, string schoolId);
    }
}
