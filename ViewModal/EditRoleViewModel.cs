using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication12.Models
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            User = new List<string>();    
        }


        public string id { get; set; } //db id string ma save huncha
        [Required(ErrorMessage ="Role Name Is Required")]
        public string RoleName { get; set; }

        public List<string> User { get; set; }
    }
}
