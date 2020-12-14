using System.ComponentModel.DataAnnotations;

namespace WebSchool.Models.RegistrationLink
{
    public class GenerateLinksInputModel
    {
        [Required]
        public string Role { get; set; }

        [Required]
        public string Emails { get; set; }
    }
}
