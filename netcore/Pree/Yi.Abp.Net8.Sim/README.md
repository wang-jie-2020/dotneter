# ABP + RuoYi

## 快速模板

1.简单轻量;</br>
2.低门槛;</br>
3.代码规范;</br>

## 本体(整合Yi.Abp)

领域驱动的成本有点难以估量,如果按照abp框架下的思路就过于臃肿,实践下来RuoYi框架可能更适合于行情.

- ABP基础
  - 不使用动态路由(RemoteService)
  - Mapster -> 兼容IObjectMapper
  - ORM -> SQLSugar
  - 不使用abp聚合根AggregateRoot,太繁琐
  - 不使用abp的RequestDto,不符合做法
  - Exception响应到RuoYi标准模式
  - 审计日志中的实体审计在EntityFramework模块中实现,不考虑针对再写,SQLSugar有自己的一套也不错
  - Permission + 菜单满足大多数,标准鉴权不考虑再写

- 其他基础
  - BaseEntity 提供两个足矣
  - AjaxResult到RuoYi模式,是不是包装全部再讨论
  - LocalEventBus 示例见LoginEvent
  - BackgroundWorker 示例见DevJob
  - Guid 雪花Id

- SYS模块
  - UserContext的Cache的BUG(登录刷新)
  - Token过长要削减,同时Cookie有长度限制和同源策略
  - MiniExcel的Import,Export基本也够
  - 日志组件Serilog唯一的缺憾是滚动命名不能自定义

## TODO
  - minio
  - tenant
    - 目前多租户必须切换DB,很多小系统可能不够资源 
    - 租户的DB通过租户名称得到,不合适
  - SQLSugar 
    - 暴露出客户端感觉更合适
    - 相较存在SQL过长的问题,在仓储层直接做?
    - aop的做法是不是集中比较好?
  - 实体审计的SQLSugar实现?
  - 操作日志的Interceptor?
  - RefreshTokenMiddleware写不写再想想
  
  - MiniProfiler,SkyWalking
  - Cookie、Localstorage
  - 国际化
    - BusinessException、FriendlyException
    - DateTime.Now UTC.Now
    - 前后端多语言
    - Excel
  - 搞一个EFCore版?
  - Guid 继续改雪花




