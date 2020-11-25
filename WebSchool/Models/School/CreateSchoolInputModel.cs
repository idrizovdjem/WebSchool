using System.ComponentModel.DataAnnotations;

namespace WebSchool.Models.School
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
