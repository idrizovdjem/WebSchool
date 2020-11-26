﻿using System.ComponentModel.DataAnnotations;

namespace WebSchool.Models.User
{
    public class LoginUserInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}
