using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebSchool.Data.Models
{
    public class Subject
    {
        public Subject()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Users = new HashSet<UserSubject>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required]
        public string SchoolId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<UserSubject> Users { get; set; }
    }
}
