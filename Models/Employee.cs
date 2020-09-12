using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication12.Models
{
    public class Employee 
    {

        public int Id { get; set; }
        [Required, MaxLength(50,ErrorMessage ="Name cannot Exceed 5 charater")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",ErrorMessage = "Invalid email format")]
        [Display(Name ="Office Email")]
        public string Email { get; set; }
        [Required]
        public dept? Department { get; set; }
        public string PhotoPath { get; set; }
        
        
    }
}
