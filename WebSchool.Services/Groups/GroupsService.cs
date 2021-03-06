﻿using System;
using System.Linq;
using System.Threading.Tasks;

using WebSchool.Data;
using WebSchool.Data.Models;
using WebSchool.Services.Posts;
using WebSchool.ViewModels.Group;
using WebSchool.Common.Constants;
using WebSchool.Common.Enumerations;
using WebSchool.Services.Assignments;

namespace WebSchool.Services.Groups
{
    public class GroupsService : IGroupsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IPostsService postsService;
        private readonly IAssignmentsService assignmentsService;

        public GroupsService(
            ApplicationDbContext dbContext, 
            IPostsService postsService,
            IAssignmentsService assignmentsService)
        {
            this.dbContext = dbContext;
            this.postsService = postsService;
            this.assignmentsService = assignmentsService;
        }

        public async Task AddUserToGroupAsync(string userId, string groupId, GroupRole role)
        {
            var userRole = dbContext.Roles
                .FirstOrDefault(r => r.Name.ToLower() == role.ToString().ToLower());

            var userGroup = new UserGroup()
            {
                UserId = userId,
                GroupId = groupId,
                Role = userRole
            };

            await dbContext.UserGroups.AddAsync(userGroup);
            await dbContext.SaveChangesAsync();
        }

        public async Task ChangeNameAsync(ChangeGroupNameInputModel input)
        {
            var group = dbContext.Groups.Find(input.Id);
            if (group == null)
            {
                return;
            }

            group.Name = input.Name.Trim();
            await dbContext.SaveChangesAsync();
        }

        public async Task<string> CreateAsync(string userId, string name)
        {
            var group = new Group()
            {
                Name = name.Trim(),
                OwnerId = userId,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false
            };

            await dbContext.Groups.AddAsync(group);
            await dbContext.SaveChangesAsync();

            return group.Id;
        }

        public GroupViewModel GetGroupContent(string userId, string groupId)
        {
            var groupViewModel = dbContext.Groups
                .Where(g => g.Id == groupId)
                .Select(g => new GroupViewModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    NewestPosts = postsService.GetNewestPosts(userId, g.Id, PostConstants.MostPopularPostsCount)
                })
                .FirstOrDefault();

            if(groupViewModel == null)
            {
                return null;
            }

            var roleId = dbContext.UserGroups
                .FirstOrDefault(ug => ug.UserId == userId && ug.GroupId == groupViewModel.Id)?.RoleId;

            var role = dbContext.Roles.Find(roleId).Name;
            groupViewModel.UserRole = (GroupRole)Enum.Parse(typeof(GroupRole), role);

            return groupViewModel;
        }

        public string GetIdByName(string name)
        {
            return dbContext.Groups
                .FirstOrDefault(g => g.Name == name)?.Id;
        }

        public string GetName(string groupId)
        {
            return dbContext.Groups
                .FirstOrDefault(g => g.Id == groupId)?.Name;
        }

        public string GetOwnerId(string groupId)
        {
            return dbContext.Groups
                .Where(g => g.Id == groupId)
                .Select(g => g.OwnerId)
                .FirstOrDefault();
        }

        public GroupAssignmentResultViewModel[] GetResults(string id, string userId)
        {
            var giveAssignmentIds = dbContext.GivenAssignments
                .Where(g => g.GroupId == id)
                .Select(g => g.Id)
                .ToArray();

            var results = dbContext.AssignmentResults
                .Where(ar => giveAssignmentIds.Contains(ar.groupAssignmentId) && ar.IsSolved == true && ar.StudentId == userId)
                .Select(ar => new GroupAssignmentResultViewModel()
                {
                    AssignmentId = dbContext.GivenAssignments
                        .Where(ga => ga.Id == ar.groupAssignmentId)
                        .Select(ga => ga.AssignmentId)
                        .FirstOrDefault(),
                    Points = ar.Points,
                })
                .ToArray();

            foreach(var result in results)
            {
                var assignmentModel = assignmentsService.GetById(result.AssignmentId);
                result.MaxPoints = assignmentModel.AllPoints;
                result.Title = assignmentModel.Title;
            }

            return results;
        }

        public GroupItemViewModel[] GetUserGroups(string userId)
        {
            return dbContext.UserGroups
                .Where(ug => ug.UserId == userId)
                .Select(ug => new GroupItemViewModel()
                {
                    Id = ug.GroupId,
                    Name = ug.Group.Name
                })
                .ToArray();
        }

        public bool GroupExists(string id)
        {
            return dbContext.Groups.Any(g => g.Id == id);
        }

        public bool IsGroupNameAvailable(string name)
        {
            return dbContext.Groups.All(g => g.Name != name);
        }
    }
}
