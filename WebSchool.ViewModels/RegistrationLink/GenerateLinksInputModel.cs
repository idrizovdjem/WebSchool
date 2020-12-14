using System.ComponentModel.DataAnnotations;

namespace WebSchool.ViewModels.RegistrationLink
{
    public class GenerateLinksInputModel
    {
        [Required]
        public string Role { get; set; }

        [Required]
        public string Emails { get; set; }
    }
}
