using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication12.Models
{
    public class AppDB : DbContext
    {

        public AppDB(DbContextOptions<AppDB> options ) :base(options)
        {


        }

        public DbSet<Employee> Emplopyess { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }

    }
}
