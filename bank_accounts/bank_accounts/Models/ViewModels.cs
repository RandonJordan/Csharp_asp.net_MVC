using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bank_accounts.Models
{
    public class RegisterUser
    {
        [Required(ErrorMessage="First name is required")]
        [MinLength(2, ErrorMessage="First name must be at least 2 characters")]
        [RegularExpression("^[a-zA-Z ]*$")]
        [Display(Name = "First Name")]
        public string FirstName {get; set;}
        [Required(ErrorMessage="Last Name is required")]
        [MinLength(2, ErrorMessage="Last Name must be at least 2 characters")]
        [RegularExpression("^[a-zA-Z ]*$")]
        [Display(Name = "Last Name")]
        public string LastName {get; set;}
        [Required(ErrorMessage="Email name is required")]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email {get; set;}
        [Required(ErrorMessage="Password is required")]
        [MinLength(8, ErrorMessage="Password must be 8 characters long")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password {get; set;}
        [Compare("Password", ErrorMessage="Password and password confirmation do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string PasswordConfirm {get; set;}
    }
    public class LoginUser
    {
        [Required(ErrorMessage="Email name is required")]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email {get; set;}
        [Required(ErrorMessage="Password is required")]
        [MinLength(8, ErrorMessage="Password must be 8 characters long")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password {get; set;}
    }
}