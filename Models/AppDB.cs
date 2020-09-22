using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

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

            foreach (var foreign in modelBuilder.Model.GetEntityTypes().SelectMany(e=>e.GetForeignKeys()))
            {
                foreign.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

    }
}
