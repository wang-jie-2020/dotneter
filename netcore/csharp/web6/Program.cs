using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;

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

            builder.Services.AddMemoryCache();

            builder.Services.AddRouting(o =>
            {
                o.LowercaseUrls = true;
                o.LowercaseQueryStrings = true;
            });

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {

            });

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();

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

            using (var scope = app.Services.CreateScope())
            {
                var options = scope.ServiceProvider.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
            }

            Console.Out.WriteLineAsync($"{CultureInfo.CurrentCulture}---{DateTime.Now}");

            app.Map("/ping", app =>
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync("pong");
                });
            });

            app.MapControllers();

            //var url = @"https://10.202.66.13:9545/oauth/oauth/token?client_id=client&client_secret=secret&grant_type=client_credentials";

            //var handler = new HttpClientHandler
            //{
            //    ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true,
            //    ClientCertificateOptions = ClientCertificateOption.Manual,

            //};

            //var webRequest = new HttpClient(handler);
            //var result = webRequest.PostAsync(url, null).Result;

            //var token = result.Content.ReadAsStringAsync();


            app.Run();
        }
    }
}