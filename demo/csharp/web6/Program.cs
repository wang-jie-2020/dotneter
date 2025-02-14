using System.Reflection;
using System.Reflection.PortableExecutable;

namespace web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //UseSetting指定启动程序集不再支持,代码出来也是报错的
            //builder.WebHost.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, $"HostingStartupAssemblies;{Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty}");
            //builder.WebHost.UseSetting(WebHostDefaults.HostingStartupAssembliesKey, $"HostingStartupAssemblies");

            //var appIni = builder.Configuration[WebHostDefaults.HostingStartupAssembliesKey];
            //Console.WriteLine(appIni?.ToString());

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            
            //Console.WriteLine("--切入--");
            //using (var scope = app.Services.CreateScope())
            //{
            //    var congigurationExtra = app.Configuration["extra-service-name"]?.ToString();
            //    Console.WriteLine(congigurationExtra);

            //    var serviceExtra = scope.ServiceProvider.GetService<ExtraService>();
            //    Console.WriteLine(serviceExtra?.Name);
            //}

            // app.Use(async (context, next) =>
            // {
            //     await next.Invoke();
            // });

            app.Map("/ping", app =>
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("pong");
                });
            });
            
            app.MapControllers();
            
            app.Run();
        }
    }
}