using WebSchool.Models.Post;
using System.Collections.Generic;

namespace WebSchool.Services.Contracts
{
    public interface IPostsService
    {
        ICollection<PostViewModel> GetPosts(string schoolId, int page);
    }
}
