﻿using System.ComponentModel.DataAnnotations;

namespace Eci_website.ViewModel
{
    public class CreateViewModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage ="Parolalar Eşleşmiyor.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}