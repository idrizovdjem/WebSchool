﻿using System.ComponentModel.DataAnnotations;

using WebSchool.Common.Constants;

namespace WebSchool.ViewModels.Assignment
{
    public class CreateAssignmentInputModel
    {
        [Required(ErrorMessage = AssignmentConstants.TitleIsRequiredMessage)]
        [MinLength(AssignmentConstants.MinimumTitleLength, ErrorMessage = AssignmentConstants.TitleLengthMessage)]
        [MaxLength(AssignmentConstants.MaximumTitleLength, ErrorMessage = AssignmentConstants.TitleLengthMessage)]
        public string Title { get; set; }

        public AssignmentQuestionInputModel[] Questions { get; set; }
    }
}
