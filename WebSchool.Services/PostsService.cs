﻿using System;
using System.Linq;
using System.Threading.Tasks;

using WebSchool.Data;
using WebSchool.ViewModels.Post;

using WebSchool.Data.Models;
using WebSchool.Services.Contracts;
using WebSchool.ViewModels.Comment;

namespace WebSchool.Services
{
    public class PostsService : IPostsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ICommentsService commentsService;

        public PostsService(ApplicationDbContext context, ICommentsService commentsService)
        {
            this.dbContext = context;
            this.commentsService = commentsService;
        }

        public async Task CreateAsync(CreatePostInputModel input, string userId)
        {
            var post = new Post()
            {
                Content = input.Content,
                Title = input.Title,
                GroupId = input.GroupId,
                CreatorId = userId,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false
            };

            await dbContext.Posts.AddAsync(post);
            await dbContext.SaveChangesAsync();
        }

        public PostViewModel[] GetAll(string groupId)
        {
            return dbContext.Posts
                .Where(p => p.GroupId == groupId && p.IsDeleted == false)
                .Select(p => new PostViewModel()
                {
                    Id = p.Id,
                    Title = p.Title.Length < 50 ? p.Title : p.Title.Substring(0, 50) + "...",
                    Creator = p.Creator.Email,
                    CreatedOn = p.CreatedOn,
                    Content = p.Content.Substring(0, 25) + "...",
                    Comments = commentsService.GetPostComments(p.Id)
                })
                .ToArray();
        }

        public PostViewModel GetById(string userId, string postId)
        {
            return dbContext.Posts
                .Where(p => p.Id == postId && p.IsDeleted == false)
                .Select(p => new PostViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Creator = p.Creator.Email,
                    CreatedOn = p.CreatedOn,
                    Content = p.Content,
                    Comments = commentsService.GetPostComments(p.Id),
                    IsCreator = p.CreatorId == userId
                })
                .FirstOrDefault();
        }

        public PostViewModel[] GetNewestPosts(string userId, string groupId, int count = 10)
        {
            return dbContext.Posts
                .Where(p => p.GroupId == groupId && p.IsDeleted == false)
                .OrderByDescending(p => p.CreatedOn)
                .Take(count)
                .Select(p => new PostViewModel()
                {
                    Id = p.Id,
                    Title = p.Title.Length < 50 ? p.Title : p.Title.Substring(0, 50) + "...",
                    Content = p.Content.Length < 500 ? p.Content : p.Content.Substring(0, 500) + "...",
                    CreatedOn = p.CreatedOn,
                    Creator = p.Creator.Email,
                    IsCreator = p.CreatorId == userId,
                    Comments = commentsService.GetPostComments(p.Id)
                })
                .ToArray();
        }
    }
}
