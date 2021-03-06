﻿using System.ComponentModel.DataAnnotations;


namespace CoreTest.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
