using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceSite.Models
{
    public class UserAccount
    {
        [Key]
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }

    /// <summary>
    /// A view model a model built for a view, can still use model binding without having to worry about security vulnerabilities as much.
    /// </summary>
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; }

        [Compare(nameof(Email))]
        [Required]
        [Display(Name = "Confirm Email")]
        public string ConfirmEmail { get; set; }

        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]                           // {2} is the second argument number, {1} is the 1st argument number
        [StringLength(120, MinimumLength = 8, ErrorMessage = "Password must be between {2} and {1}")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Date)] // Time is ignored
        public DateTime? DateOfBirth { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username or Email")]
        public string UsernameOfEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
