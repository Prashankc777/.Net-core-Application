using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication12.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;


        public EmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee(){ Id = 1, Name = "Prashan", Email ="prashankc777@gmail.com", Department = dept.hr},
                new Employee(){ Id = 2, Name = "Rubina", Email ="rubina@gmail.com", Department = dept.it},
                new Employee(){ Id = 3, Name = "sha", Email ="sha@gmail.com", Department = dept.hr}
            };


        }

        public Employee add(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
           Employee employeee =  _employeeList.FirstOrDefault(e => e.Id == id);
            if (employeee != null)
            {
                _employeeList.Remove(employeee);

            }
            return employeee;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int id)
        {
           return _employeeList.FirstOrDefault(q => q.Id == id);  
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employeee = _employeeList.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (employeee != null)
            {
                employeee.Name = employeeChanges.Name;
                employeee.Email = employeeChanges.Email;
                employeee.Department = employeeChanges.Department;                   

            }
            return employeee;
        }
    }
}
