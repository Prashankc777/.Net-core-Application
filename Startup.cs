using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApplication12.Models;

namespace WebApplication12
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config )
        {
            _config = config;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = _config.GetConnectionString("DefaultConnection");
             services.AddDbContextPool<AppDB>(options =>
                options.UseSqlServer(connectionString));
            services.AddMvc();
            services.AddScoped<IEmployeeRepository, SqlEmployeeRepository>();
        }

       
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
           
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=details}/{id?}");

            });


            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync($"hello world");
            //});
        }
    }
}
