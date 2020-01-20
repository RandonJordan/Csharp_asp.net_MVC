using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace dojo_activity_center.Models
{
    public class RegisterUser
    {
        [Required(ErrorMessage="First name is required")]
        [MinLength(2, ErrorMessage="First name must be at least 2 characters")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "First name can only contain alphabets")]
        [Display(Name = "First Name")]
        public string FirstName {get; set;}
        [Required(ErrorMessage="Last Name is required")]
        [MinLength(2, ErrorMessage="Last Name must be at least 2 characters")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Last name can only contain alphabets")]
        [Display(Name = "Last Name")]
        public string LastName {get; set;}
        [Required(ErrorMessage="Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email Address")]
        public string Email {get; set;}
        [Required(ErrorMessage="Password is required")]
        [MinLength(8, ErrorMessage="Password must be 8 characters long")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$", ErrorMessage = "Password must contain 1 letter, 1 number and 1 special character")]
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
        [Required(ErrorMessage="Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email Address")]
        public string LogEmail {get; set;}
        [Required(ErrorMessage="Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string LogPassword {get; set;}
    }
    public class NewActivity
    {
        [Required]
        [MinLength(2, ErrorMessage="Title must be at least 2 characters")]
        [Display(Name = "Title")]
        public string Title {get; set;}
        [Required]
        [Display(Name = "Date")]
        [CheckDateRange]
        public DateTime Date {get; set;}
        [Required]
        [Display(Name = "Time")]
        [RegularExpression(@"\b((1[0-2]|0?[1-9]):([0-5][0-9]) ([AaPp][Mm]))", ErrorMessage="Please make sure your time is chosen hh/mm AM or PM")]
        public string Time {get; set;}
        [Required]
        [Display(Name = "Duration:")]
        [RegularExpression(@"^[+]?\d+([.]\d+)?$", ErrorMessage = "Only positive numbers allowed")]
        public int Duration {get; set;}
        [Required]
        [MinLength(10, ErrorMessage= "Description must be at leat 10 characters long")]
        public string Description {get; set;}
    }
    public class DashboardModels
    {
        public List<Activity> AllActivities {get; set;}
        public User User {get; set;}
        public List<User> AllUsers {get; set;}
        public List<Activity> JoinedActivities {get; set;}
    }
    public class CheckDateRangeAttribute: ValidationAttribute {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
        DateTime dt = (DateTime)value;
        if (dt >= DateTime.UtcNow) {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage ?? "Make sure your date is in the future");
        }
    }
}