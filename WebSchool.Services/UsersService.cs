﻿using System.Linq;

using WebSchool.Data;
using WebSchool.Services.Contracts;
using WebSchool.Common.Enumerations;
using System;

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
    }
}
