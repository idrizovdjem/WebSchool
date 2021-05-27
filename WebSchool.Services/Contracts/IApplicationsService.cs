﻿using System.Threading.Tasks;

using WebSchool.Common.Enumerations;
using WebSchool.ViewModels.Application;

namespace WebSchool.Services.Contracts
{
    public interface IApplicationsService
    {
        ApplicationStatus GetApplicationStatus(string userId, string groupId);

        Task ApplyAsync(string userId, string groupId);

        ApplicationViewModel[] GetApplications(string groupId);
    }
}
