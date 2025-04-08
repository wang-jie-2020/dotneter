using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Demo.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo
{
    public class Startup
    {
        public static string Ver = "1";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //var controllersAssembly = Assembly.LoadFrom($"{AppDomain.CurrentDomain.BaseDirectory}\\UnitOfWorkContextRepository.Demo.dll");

            //services.AddMvc(options =>
            //{

            //}).AddApplicationPart(controllersAssembly);

            var connectionString = Configuration.GetConnectionString("Default");

            services.AddDbContext<AppDbContext>(options => { options.UseSqlServer(connectionString, t => { }); });

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger().UseSwaggerUI();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            if (Ver == "1")
            {
                DatabaseInitializer1.Seed(app);
            }
            else
            {
                DatabaseInitializer2.Seed(app);
            }
        }
    }
}
