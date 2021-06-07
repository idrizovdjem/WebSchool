using System.ComponentModel.DataAnnotations;
using WebSchool.Common.Constants;

namespace WebSchool.ViewModels.Group
{
    public class ChangeGroupNameInputModel
    {
        [Required]
        public string Id { get; set; }

        [Required(ErrorMessage = GroupConstants.GroupNameIsRequiredMessage)]
        [MinLength(GroupConstants.MinimumNameLength, ErrorMessage = GroupConstants.InvalidNameLengthMessage)]
        [MaxLength(GroupConstants.MaximumNameLength, ErrorMessage = GroupConstants.InvalidNameLengthMessage)]
        public string Name { get; set; }
    }
}
