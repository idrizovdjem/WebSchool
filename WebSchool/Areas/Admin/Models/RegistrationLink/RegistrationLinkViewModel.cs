using System;

namespace WebSchool.Areas.Admin.Models.RegistrationLink
{
    public class RegistrationLinkViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Role { get; set; }

        public string IsUsed { get; set; }
    }
}
