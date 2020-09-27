using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication12.Models;
using WebApplication12.ViewModal;

namespace WebApplication12.Controllers
{
   
    public class HomeController : Controller
    {
        public readonly IEmployeeRepository EmployeeRepository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IEmployeeRepository employee, IHostingEnvironment hostingEnvironment)
        {
            this.EmployeeRepository = employee;
            this._hostingEnvironment = hostingEnvironment;
        }

        public ViewResult Index()
        {
            var model = EmployeeRepository.GetAllEmployee();
            return View(model);

        }
        [Route("details/{id?}")]
        public ViewResult details(int? id)
        {
            Debug.Assert(id != null, nameof(id) + " != null");
            var employee = EmployeeRepository.GetEmployee(id.Value);
            if (employee is null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }
            var home = new HomeDetails()
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
            if (!ModelState.IsValid) return View();
            var uniqueFilename = ProcessUploadedFile(employee);

            var newEmployee = new Employee
            {
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                PhotoPath = uniqueFilename
            };

            EmployeeRepository.add(newEmployee);

            return RedirectToAction("details", new { id = newEmployee.Id });


        }

        [HttpGet]

      
        public ViewResult Edit(int id)
        {
            var employee = EmployeeRepository.GetEmployee(id);
            var viewmodel = new EmployeeEditViewModel
            {
                id = employee.Id,
                Name = employee.Name,
                Department = employee.Department,
                Email = employee.Email,
                ExistingPhotoPath = employee.PhotoPath

            };
            return View(viewmodel);

        }

        [HttpPost]

        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var employee = EmployeeRepository.GetEmployee(model.id);

            employee.Name = model.Name;
            employee.Email = model.Email;
            employee.Department = model.Department;
            if (model.Photo != null)
            {

                if (model.ExistingPhotoPath != null)
                {
                    var filePath = Path.Combine(_hostingEnvironment.WebRootPath,
                        "images", model.ExistingPhotoPath);
                    System.IO.File.Delete(filePath);
                }

                employee.PhotoPath = ProcessUploadedFile(model);
            }


            var updatedEmployee = EmployeeRepository.Update(employee);
            return RedirectToAction("index");


        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo is null) return null;
            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);
            model.Photo.CopyTo(fileStream);
            return uniqueFileName;
        }
    }
}
