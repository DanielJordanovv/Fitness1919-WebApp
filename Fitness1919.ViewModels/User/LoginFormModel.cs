﻿using System.ComponentModel.DataAnnotations;

namespace Fitness1919.Web.ViewModels.User
{
    public class LoginFormModel
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;


        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
        public bool IsDeleted { get; set; }
    }
}
