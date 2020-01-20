using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FormSubmission.Models
{
    public class User
    {
        [Required(ErrorMessage = "Please enter your first name")]
        [MinLength(4, ErrorMessage = "Frist name must be at least 4 characters")]
        public string FirstName { get; set; }
        [MinLength(4, ErrorMessage = "Frist name must be at least 4 characters")]
        [Required(ErrorMessage = "Enter your last name")]
        public string LastName { get; set; }
        [Range(1,120,ErrorMessage = "Enter a valid age")]
        public int Age { get; set; }
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        [Required(ErrorMessage = "Please enter an email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter a password")]
        [MinLength(8, ErrorMessage = "password must be at least 8 characters")]
        public string Password { get; set; }    
    }
}
