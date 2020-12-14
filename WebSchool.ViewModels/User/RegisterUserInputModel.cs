using System.ComponentModel.DataAnnotations;

namespace WebSchool.ViewModels.User
{
    public class RegisterUserInputModel
    {
        [Required]
        public string RegistrationLinkId { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        public string ConfirmPassword { get; set; }
    }
}
