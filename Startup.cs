using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApplication12.Models;

namespace WebApplication12
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            this._config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = _config.GetConnectionString("DefaultConnection");
            services.AddDbContextPool<AppDB>(options =>
               options.UseSqlServer(connectionString));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDB>();

            services.AddMvc(Option =>
           {
               //var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
               //Option.Filters.Add(new AuthorizeFilter(policy));

           }).AddXmlDataContractSerializerFormatters();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role")) ;
                options.AddPolicy("EditRolePolicy", policy => policy.RequireClaim("Edit Role")) ;
                options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin")) ;
            });


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
            app.UseAuthentication();

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
