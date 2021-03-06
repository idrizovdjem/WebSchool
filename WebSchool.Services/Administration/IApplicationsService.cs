﻿using System.Threading.Tasks;

using WebSchool.Common.Enumerations;
using WebSchool.ViewModels.Application;

namespace WebSchool.Services.Administration
{
    public interface IApplicationsService
    {
        ApplicationStatus GetApplicationStatus(string userId, string groupId);

        int GetCount(string groupId);

        Task ApplyAsync(string userId, string groupId);

        Task ApproveAsync(string applicantId, string groupId);

        Task DeclineAsync(string applicantId, string groupId);

        Task RemoveAsync(string applicantId, string groupId);

        ApplicationViewModel[] GetApplications(string groupId);
    }
}
