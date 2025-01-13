## Swagger

- 生成webapi文档

- 按文档展示、测试

![Swagger UI](images/swagger-ui.png)

## 包安装

- Swashbuckle.AspNetCore.Swagger

  ​	将 SwaggerDocument 对象公开为 JSON 终结点的 Swagger 对象模型和中间件。

- Swashbuckle.AspNetCore.SwaggerGen

  ​	从路由、控制器和模型直接生成 SwaggerDocument 对象的 Swagger 生成器。 它通常与 Swagger 终结点中间件结合，以自动公开 Swagger JSON。

- Swashbuckle.AspNetCore.SwaggerUI

  Swagger UI 工具的嵌入式版本。 它解释 Swagger JSON 以构建描述 Web API 功能的可自定义的丰富体验。 它包括针对公共方法的内置测试工具。

## 配置

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
        
    // Register the Swagger generator, defining 1 or more Swagger documents
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    });
}

public void Configure(IApplicationBuilder app)
{
    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        // c.RoutePrefix = string.Empty;	//根处提供Swagger UI
    });

    app.UseRouting();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}
```

## [自定义和扩展](https://docs.microsoft.com/zh-cn/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio)

- API 信息和说明

- XML 注释

- 数据注释
- etc 