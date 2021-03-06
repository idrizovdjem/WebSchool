﻿using System.Threading.Tasks;

using WebSchool.ViewModels.Users;
using WebSchool.Common.Enumerations;

namespace WebSchool.Services.Administration
{
    public interface IMembersService
    {
        MemberViewModel GetById(string memberId, string groupId);

        Task UpdateAsync(string memberId, string groupId, GroupRole role);

        Task RemoveAsync(string memberId, string groupId);

        MemberViewModel[] GetMembers(string adminId, string groupId);

        int GetCount(string groupId);
    }
}
