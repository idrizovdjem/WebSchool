﻿using System.Threading.Tasks;

using WebSchool.ViewModels.Assignment;
using WebSchool.Common.ValidationResults;

namespace WebSchool.Services.Assignments
{
    public interface IAssignmentsService
    {
        CreatedAssignmentViewModel[] GetCreated(string userId);

        Task CreateAsync(string userId, CreateAssignmentInputModel input);

        AssignmentValidationResult ValidateAssignment(CreateAssignmentInputModel input);

        AssignmentViewModel GetById(string id);

        Task GiveAsync(GiveAssignmentInputModel input);

        GivenAssignmentViewModel[] GetGiven(string userId);
    }
}
