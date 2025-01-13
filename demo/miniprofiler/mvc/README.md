官方文档： https://miniprofiler.com/dotnet/AspDotNetCore

1、添加包 MiniProfiler.AspNetCore.Mvc和MiniProfiler.EntityFrameworkCore

2、在 Startup.cs 中的 ConfigureServices 下添加，也可以根据官方文档中的说明进行相应的配置
services.AddMiniProfiler().AddEntityFramework();

3、在 Startup.cs 中的 Configure 下添加
app.UseMiniProfiler();

4、修改 _ViewImports.cshtml 

@using StackExchange.Profiling
@addTagHelper *, MiniProfiler.AspNetCore.Mvc

5、将MiniProfiler添加到布局文件（Shared/_Layout.cshtml）中
<mini-profiler />