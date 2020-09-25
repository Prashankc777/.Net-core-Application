using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication12.Controllers
{
    public class DepartmentController : Controller
    {

        public string List()
        {
            return "list of department";
        }
        public string Department()
        {
            return "Department of department";
        }
    }
}
