using System.ComponentModel.DataAnnotations;

namespace WebSchool.ViewModels.School
{
    public class CreateSchoolInputModel
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}
