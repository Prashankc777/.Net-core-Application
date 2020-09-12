using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication12.Models;
using WebApplication12.ViewModal;

namespace WebApplication12.Controllers
{      
    public class HomeController : Controller 
    {
        public readonly IEmployeeRepository EmployeeReposito;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepository _employee, IHostingEnvironment hostingEnvironment)
        {
            this.EmployeeReposito = _employee;
            this.hostingEnvironment = hostingEnvironment;
        }  
        public ViewResult Index()
        {
            var model = EmployeeReposito.GetAllEmployee();
            return View(model);

        }
        [Route("details/{id?}")]
        public ViewResult details(int? id)
        {
            HomeDetails home = new HomeDetails()
            {
                Employee = EmployeeReposito.GetEmployee(id??1),
                PageTitle = "Employee Details"

             };
           
            return View(home);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel employee)
        {
            if (ModelState.IsValid)
            {
                string uniqueFilename = null;
                if (employee.Photo !=null)
                {
                    string UploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFilename= Guid.NewGuid().ToString() + "_" + employee.Photo.FileName;
                    string filepath = Path.Combine(UploadsFolder, uniqueFilename);
                    employee.Photo.CopyTo(new FileStream(filepath, FileMode.Create));
                }

                Employee newEmployee = new Employee
                {
                    Name = employee.Name,
                    Email = employee.Email,
                    Department = employee.Department,
                    PhotoPath = uniqueFilename
                };

                EmployeeReposito.add(newEmployee);

                return RedirectToAction("details", new { id = newEmployee.Id });

            }
            return View();    
        
        }
    }
}
