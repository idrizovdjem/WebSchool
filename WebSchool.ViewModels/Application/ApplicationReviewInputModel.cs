using System.ComponentModel.DataAnnotations;

namespace WebSchool.ViewModels.Application
{
    public class ApplicationReviewInputModel
    {
        [Required]
        public string GroupId { get; set; }

        [Required]
        public string ApplicantId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
