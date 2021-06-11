using System;
using System.ComponentModel.DataAnnotations;

namespace WebSchool.Data.Models
{
    public class Notification
    {
        public Notification()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string Message { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsActive { get; set; }
    }
}
