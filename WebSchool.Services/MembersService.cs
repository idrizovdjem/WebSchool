using System.Linq;
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

        public MembersService(ApplicationDbContext dbContext, IUsersService usersService)
        {
            this.dbContext = dbContext;
            this.usersService = usersService;
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
