using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication12.Models
{
    public class SqlEmployeeRepository : IEmployeeRepository
    {
        private  readonly AppDB context;

        public SqlEmployeeRepository(AppDB context)
        {
            this.context = context;

        }

        public Employee add(Employee employee)
        {
            context.Emplopyess.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employe  = context.Emplopyess.Find(id);
            if (employe !=null)
            {
                context.Emplopyess.Remove(employe);
                context.SaveChanges();

            }
            return employe;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return context.Emplopyess;
        }

        public Employee GetEmployee(int id)
        {
            return context.Emplopyess.Find(id);
        }

        public Employee Update(Employee employeeChanges)
        {
             var employee = context.Emplopyess.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return employeeChanges;
        }
    }
}
