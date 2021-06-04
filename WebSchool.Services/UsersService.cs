using System;
using System.Linq;

using WebSchool.Data;
using WebSchool.Services.Contracts;
using WebSchool.Common.Enumerations;

namespace WebSchool.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext dbContext;

        public UsersService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public GroupRole GetRoleInGroup(string userId, string groupId)
        {
            var roleId = dbContext.UserGroups
                .First(ug => ug.UserId == userId && ug.GroupId == groupId)
                .RoleId;

            var roleName = dbContext.Roles
                .Find(roleId).Name;

            var role = (GroupRole)Enum.Parse(typeof(GroupRole), roleName);
            return role;
        }

        public bool IsUserGroupCreator(string userId, string groupId)
        {
            return dbContext.Groups
                .Any(g => g.OwnerId == userId);
        }

        public bool IsUserInGroup(string userId, string groupId)
        {
            return dbContext.UserGroups
                .Any(ug => ug.UserId == userId && ug.GroupId == groupId);
        }

        public bool ValidatePostRemove(string userId, string postId)
        {
            var post = dbContext.Posts
                .FirstOrDefault(p => p.Id == postId && p.IsDeleted == false);

            if(post == null)
            {
                return false;
            }

            if(post.CreatorId == userId)
            {
                return true;
            }

            return IsUserGroupCreator(userId, post.GroupId);
        }
    }
}
