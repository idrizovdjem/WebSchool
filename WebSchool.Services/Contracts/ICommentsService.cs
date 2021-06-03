﻿using System.Threading.Tasks;

using WebSchool.ViewModels.Comment;

namespace WebSchool.Services.Contracts
{
    public interface ICommentsService
    {
        CommentViewModel[] GetPostComments(string userId, string postId);

        Task CreateAsync(string postId, string content, string userId);

        Task RemoveAllPostCommentsAsync(string postId);

        int GetCount(string postId);

        Task<bool> RemoveAsync(string userId, int commentId);
    }
}
