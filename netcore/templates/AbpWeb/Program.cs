using AbpWeb.Interceptors;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Castle.DynamicProxy;
using Volo.Abp.Modularity;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseAutofac();
await builder.Services.AddApplicationAsync<MainModule>(); 

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
await app.InitializeAsync();
app.Run();

// [DependsOn(typeof(AbpAutofacModule))]
internal class MainModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistered(c =>
        {
            if (typeof(SimpleInterceptionTargetClass) == c.ImplementationType)
            {
                c.Interceptors.Add<SimpleInterceptor>();
            }
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // DependsOn 也可
        context.Services.AddTransient(typeof(AbpAsyncDeterminationInterceptor<>));
        
        context.Services.AddTransient<SimpleInterceptor>();
        context.Services.AddTransient<SimpleInterceptionTargetClass>();
    }

    public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var sc = context.ServiceProvider.GetRequiredService<SimpleInterceptionTargetClass>();
        sc.DoIt();
        
        return Task.CompletedTask;
    }
}