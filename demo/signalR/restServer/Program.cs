using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using RestServer.Hubs;

namespace RestServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSignalR(options =>
            {
                //options.AddFilter<>();
            })
            .AddHubOptions<ChatHub>(hubOptions =>
            {
                //hubOptions.AddFilter<>();
            });

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<ChatHub>("/chatHub", options =>
            {

            })
            .RequireCors(policy =>
            {
                //policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                policy.WithOrigins("http://localhost:9528").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            });

            app.Run();
        }
    }
}