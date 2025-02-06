using System.Reflection;
using System.Reflection.PortableExecutable;

namespace web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();

            app.Map("/ping", app => { app.Run(async context => { await context.Response.WriteAsync("pong"); }); });

            app.MapControllers();

            app.Run();
        }
    }
}