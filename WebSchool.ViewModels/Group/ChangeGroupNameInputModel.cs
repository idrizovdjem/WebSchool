using System.ComponentModel.DataAnnotations;

namespace WebSchool.ViewModels.Group
{
    public class ChangeGroupNameInputModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [MinLength(5), MaxLength(250)]
        public string Name { get; set; }
    }
}
