using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication12.Models
{
   public  interface IEmployeeRepository
    {
        Employee GetEmployee(int id);

        IEnumerable<Employee> GetAllEmployee();

        Employee add(Employee employee);

        Employee Update(Employee employeeChanges);
        Employee Delete(int id);

    }
}
