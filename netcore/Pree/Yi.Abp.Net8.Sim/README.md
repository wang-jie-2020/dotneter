# todo

一、代码清理
1.Net代码清理(不需要的路由)
2.Vue代码清理(不需要的页面)
3.整理下DTO

二、编码
1.MiniExcel当作第一类,Magicodes额外封装专门模块
2.ajaxResult
3.ExceptionFilter重写?
4.Guid 改雪花?
5.oss

三、特性
~~1.token~~
~~(1)cookie 存不下~~
~~(2)RefreshTokenMiddleware不起作用~~

2.用户、角色、菜单,缓存机制漏洞

3.Tenant 
(1)租户切换---通过租户名称得到租户连接信息不合适
(2)必须租户切库?

4.SqlSugar
(1)默认赋值
(2)过长的SQL的PageSize是否需要暴露
(3)和miniProfiler集成,和SkyWalking集成

5.Permissions 
(1)过滤的结构设计
(2)前后端的一致性如何保证

6.日志问题
(1)审计日志(含实体)
(2)登录日志
(3)操作日志--是不是应该考虑做Interceptor
(4)符合标准的日志记录

7.监控
(1)profiler
(2)skywalking

8.国际化 
(1)错误处理
(2)DateTime.Now 会不会影响国际化?
(3)Excel重构模块
(4)前后端国际化、多语言





 




