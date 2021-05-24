using System.ComponentModel.DataAnnotations;

namespace WebSchool.ViewModels.Group
{
    public class CreateGroupInputModel
    {
        [Required]
        [MinLength(5), MaxLength(250)]
        public string Name { get; set; }
    }
}
