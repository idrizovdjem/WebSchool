using System.Threading.Tasks;

using WebSchool.ViewModels.Users;
using WebSchool.Common.Enumerations;

namespace WebSchool.Services.Contracts
{
    public interface IMembersService
    {
        MemberViewModel GetById(string memberId, string groupId);

        Task UpdateAsync(string memberId, string groupId, GroupRole role);
    }
}
