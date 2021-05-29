﻿using System.Linq;
using System.Threading.Tasks;

using WebSchool.Data;
using WebSchool.ViewModels.Users;
using WebSchool.Services.Contracts;
using WebSchool.Common.Enumerations;

namespace WebSchool.Services
{
    public class MembersService : IMembersService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IUsersService usersService;
        private readonly IApplicationsService applicationsService;

        public MembersService(ApplicationDbContext dbContext, IUsersService usersService, IApplicationsService applicationsService)
        {
            this.dbContext = dbContext;
            this.usersService = usersService;
            this.applicationsService = applicationsService;
        }

        public MemberViewModel GetById(string memberId, string groupId)
        {
            return dbContext.UserGroups
                .Where(x => x.UserId == memberId && x.GroupId == groupId)
                .Select(x => new MemberViewModel()
                {
                    Id = x.User.Id,
                    Email = x.User.Email,
                    Role = usersService.GetRoleInGroup(memberId, groupId).ToString(),
                    GroupId = groupId
                })
                .FirstOrDefault();
        }

        public async Task RemoveAsync(string memberId, string groupId)
        {
            var userGroup = dbContext.UserGroups
                .FirstOrDefault(x => x.UserId == memberId && x.GroupId == groupId);

            if(userGroup == null)
            {
                return;
            }

            await applicationsService.RemoveAsync(memberId, groupId);

            dbContext.UserGroups.Remove(userGroup);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(string memberId, string groupId, GroupRole role)
        {
            var memberGroup = dbContext.UserGroups
                .FirstOrDefault(x => x.UserId == memberId && x.GroupId == groupId);

            var roleId = dbContext.Roles
                .FirstOrDefault(x => x.Name.ToLower() == role.ToString().ToLower()).Id;

            memberGroup.RoleId = roleId;
            await dbContext.SaveChangesAsync();
        }
    }
}
