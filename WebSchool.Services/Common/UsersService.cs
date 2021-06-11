using System;
using System.Linq;

using WebSchool.Data;
using WebSchool.Common.Enumerations;

namespace WebSchool.Services.Common
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext dbContext;

        public UsersService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public string GetEmail(string id)
        {
            return dbContext.Users
                .Where(u => u.Id == id)
                .Select(u => u.Email)
                .FirstOrDefault();
        }

        public GroupRole GetRoleInGroup(string userId, string groupId)
        {
            var roleId = dbContext.UserGroups
                .First(ug => ug.UserId == userId && ug.GroupId == groupId)
                .RoleId;

            var roleName = dbContext.Roles.Find(roleId).Name;
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

        public bool ValidateCommentRemove(string userId, string commentId)
        {
            var comment = dbContext.Comments
                .FirstOrDefault(c => c.Id == commentId && c.IsDeleted == false);

            if(comment == null)
            {
                return false;
            }

            return comment.CreatorId == userId;
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
