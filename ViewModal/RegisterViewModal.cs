using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication12.Utilites;

namespace WebApplication12.ViewModal
{
    public class RegisterViewModal
    {
        [Required]
        [EmailAddress]   
        [Remote (action: "IsEmailInUse", controller: "Account")]
        [ValidEmailDomain ( _allowedDomain : "kc.com", ErrorMessage ="Email domain must be kc.com")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",  ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string City { get; set; }
    }
}
