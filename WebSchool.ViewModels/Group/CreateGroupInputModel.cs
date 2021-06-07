using System.ComponentModel.DataAnnotations;

using WebSchool.Common.Constants;

namespace WebSchool.ViewModels.Group
{
    public class CreateGroupInputModel
    {
        [Required(ErrorMessage = GroupConstants.GroupNameIsRequiredMessage)]
        [MinLength(GroupConstants.MinimumNameLength, ErrorMessage = GroupConstants.InvalidNameLengthMessage)]
        [MaxLength(GroupConstants.MaximumNameLength, ErrorMessage = GroupConstants.InvalidNameLengthMessage)]
        public string Name { get; set; }
    }
}
