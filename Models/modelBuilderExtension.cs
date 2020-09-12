using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication12.Models
{
    public  static class modelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "cde",
                    Department = dept.hr,
                    Email = "cde91@gmail.com"
                },
                 new Employee
                 {
                     Id = 2,
                     Name = "fgh",
                     Department = dept.hr,
                     Email = "cde91@gmail.com"
                 },
                  new Employee
                  {
                      Id = 3,
                      Name = "Prashan",
                      Department = dept.hr,
                      Email = "cde91@gmail.com"
                  },
                  new Employee
                  {
                      Id = 4,
                      Name = "Prashan",
                      Department = dept.hr,
                      Email = "cde91@gmail.com"
                  }

                );
        }
    }
}
