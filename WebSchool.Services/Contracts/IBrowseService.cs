﻿using WebSchool.ViewModels.Group;

namespace WebSchool.Services.Contracts
{
    public interface IBrowseService
    {
        BrowseGroupViewModel[] GetMostPopular(string userId);

        BrowseGroupViewModel[] GetGroupsContainingName(string userId, string groupName);
    }
}
