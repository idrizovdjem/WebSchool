using WebSchool.Data.Models;
using System.Threading.Tasks;
using WebSchool.ViewModels.Post;
using System.Collections.Generic;

namespace WebSchool.Services.Contracts
{
    public interface IPostsService
    {
        ICollection<PostViewModel> GetPosts(string schoolId, int page);

        Task CreatePost(string content, ApplicationUser user, string schoolId);

        Post GetPost(string postId);

        int GetMaxPages(string schoolId);
    }
}
