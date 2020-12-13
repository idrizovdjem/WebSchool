using System.ComponentModel.DataAnnotations;

namespace WebSchool.Areas.Admin.Models.RegistrationLink
{
    public class GenerateLinksInputModel
    {
        [Required]
        public string Role { get; set; }

        [Required]
        public string Emails { get; set; }
    }
}
