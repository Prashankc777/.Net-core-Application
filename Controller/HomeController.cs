using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        public readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepository _employee, IHostingEnvironment hostingEnvironment)
        {
            this._employeeRepository = _employee;
            this.hostingEnvironment = hostingEnvironment;
        }
        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);

        }
        [Route("details/{id?}")]
        public ViewResult details(int? id)
        {
            Employee employee = _employeeRepository.GetEmployee(id.Value);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }


            HomeDetails home = new HomeDetails()
            {
                Employee = employee,
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
                string uniqueFilename = ProcessUploadedFile(employee);


                Employee newEmployee = new Employee
                {
                    Name = employee.Name,
                    Email = employee.Email,
                    Department = employee.Department,
                    PhotoPath = uniqueFilename
                };

                _employeeRepository.add(newEmployee);

                return RedirectToAction("details", new { id = newEmployee.Id });

            }
            return View();


        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel empviewmodel = new EmployeeEditViewModel
            {
                id = employee.Id,
                Name = employee.Name,
                Department = employee.Department,
                Email = employee.Email,
                ExistingPhotoPath = employee.PhotoPath

            };
            return View(empviewmodel);

        }
        [HttpPost]

        public IActionResult Edit(EmployeeEditViewModel model)
        {
           
            if (ModelState.IsValid)
            {
                
                Employee employee = _employeeRepository.GetEmployee(model.id);
               
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;                
                if (model.Photo != null)
                {
                    
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath,
                            "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    
                    employee.PhotoPath = ProcessUploadedFile(model);
                }

            
                Employee updatedEmployee = _employeeRepository.Update(employee);

                return RedirectToAction("index");
            }

            return View(model);


        }

    private string ProcessUploadedFile(EmployeeCreateViewModel model)
    {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
}
}
