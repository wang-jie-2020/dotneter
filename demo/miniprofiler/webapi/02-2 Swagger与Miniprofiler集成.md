Swagger与MiniProfiler做好以后，可以集成。

```C#
//Miniprofiler配置
services.AddMvc();  //AddMiniProfiler似乎是基于MVC的。这一句不加api项目会错误
services.AddMiniProfiler(options =>
options.RouteBasePath = "/profiler"
).AddEntityFramework();
```

## 下载Swagger自定义页面

默认的index.html页面可以从如下链接下载

https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/src/Swashbuckle.AspNetCore.SwaggerUI/index.html

下载之后将这个文件放置到项目根目录下。

接下来我们需要在这个文件的头部加入如下脚本代码：

```js
<script async="async" id="mini-profiler" src="/profiler/includes.min.js?v=4.0.138+gcc91adf599" 
        data-version="4.0.138+gcc91adf599" data-path="/profiler/" 
        data-current-id="4ec7c742-49d4-4eaf-8281-3c1e0efa748a" data-ids="" data-position="Left" 
        data-authorized="true" data-max-traces="15" data-toggle-shortcut="Alt+P" 
        data-trivial-milliseconds="2.0" data-ignored-duplicate-execute-types="Open,OpenAsync,Close,CloseAsync">
</script>
```

最后我们需要配置这个index.html文件的Bulid Action为Embedded resource

## 安装自定义页面

在`Startup.cs`文件中，我们需要修改`UseSwaggerUI`中间件的配置，这里我们需要添加一个`InputStream`配置。

```c#
app.UseSwaggerUI(c =>
{
   c.RoutePrefix = "swagger";
   c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
   c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("MiniProfilerSample.index.html");
});
```

注意：这里`MiniProfilerSample`是项目的命名空间名