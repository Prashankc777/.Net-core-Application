using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication12.Models
{
    public class AppDB : IdentityDbContext<ApplicationUser>
    {

        public AppDB(DbContextOptions<AppDB> options ) :base(options)
        {


        }

        public DbSet<Employee> Emplopyess { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //For Identity
            modelBuilder.Seed();
        }

    }
}
