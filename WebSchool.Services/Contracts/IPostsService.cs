﻿using System.Threading.Tasks;

using WebSchool.ViewModels.Post;

namespace WebSchool.Services.Contracts
{
    public interface IPostsService
    {
        PostViewModel[] GetNewestPosts(string userId, string groupId, int count = 10);

        Task CreateAsync(CreatePostInputModel input, string userId);

        PostViewModel GetById(string userId, string postId);

        AdministrationPostViewModel[] GetAll(string groupId);

        Task<bool> RemoveAsync(string userId, string id);

        EditPostInputModel GetForEdit(string userId, string postId);

        Task EditAsync(EditPostInputModel input);
    }
}
