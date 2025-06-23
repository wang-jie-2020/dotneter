# ABP + RuoYi

## 本体整合

ABP的结构太过,实践难度偏高,以其中必要依赖再参考RuoYi框架更适合.</br>
这个思路下GitHub中搜索不到简易基架,在Yi.Abp上继续整合.核心思路是保证在项目尽可能直接书写代码</br>

- Yi.AspNetCore
  核心依赖类库
- Yi.Sys
  通用系统模块类库
- Yi.Admin
  Admin入口
- Utils.*
  工具包

## 思路

- ABP
  依赖过于
  依赖从Volo.Abp.AspNetCore
  Volo.Abp.AspNetCore.Authentication.JwtBearer
  Volo.Abp.AspNetCore.MultiTenancy
  Volo.Abp.AspNetCore.SignalR
  Volo.Abp.Autofac
  Volo.Abp.ObjectMapping
  Volo.Abp.EventBus
- 基本组件
  - ORM -> SQLSugar (实体审计在EntityFramework模块中实现,不考虑针对再写,SQLSugar有自己的一套也不错)
  - Redis -> FreeRedis
  - ObjectMapper -> Mapster (不考虑IObjectMapper的抽象,在业务代码中直接使用Mapster的Api)
  - Oss -> Minio
  - Excel -> MiniExcel + Magicodes
  - Uuid ->  SequentialGuidGenerator(连续Guid) + Yitter.IdGenerator(雪花Id)
- 约定和扩展
  - abp的聚合根(AggregateRoot)不适用,新增BizEntity和SimpleEntity
  - abp的动态路由(ConventionalControllers)不好管理,不使用
  - abp的鉴权模式不适用,RuoYi模式较好
  - abp的分页查询(RequestDto)不适用,新增PagedInput(web端同样)
  - abp的正确响应不包装保持,新增AjaxResult适配到RuoYi模式
  - abp的错误响应修改到AjaxResult适配到RuoYi模式(过滤器重写,web端兼容)
- 国际化
  - vue中增加vue-i18n,路由和面包屑,登录页面补了示例,其他页面不动
  - abp的本地化模式保留,但json作为嵌入资源似乎不合适项目
  - zh-CN -> zh-Hans -> zh 的关系不考虑了,约定web端zh-CN
  - DateTime.Now TimeZone 要考虑实际情况,未能进行
  - Excel 要考虑实际情况,未能进行
  - Swagger
  - MiniProfiler
- 示例LocalEventBus -> LoginEvent
  - 示例BackgroundWorker -> HelloJob
  - 示例AbpInterceptor -> OperLogInterceptor
  - User的Cache登录时刷新避免修改不生效
  - User的Token超过Cookie长度限制
  - 新增了Aop标注操作日志

- TODO
  - token claim 的 对照,,,有点乱
  - 要不要支持Authorization?Permission简化到方法标注就足够了.其他的没必要...以及在service等其他地方标准我觉得也是没必要的
  - 刷新Token??
 - EntityFrameworkCore的分支
 - 租户
   - 租户DB隔离,很多系统可能不够资源
   - 租户名称得到DB名称,约束太强不合适
 - SQLSugar
    - 暴露出客户端?SQL过长的问题在仓储层直接做?
    - aop是不是集中在web入口比较好?
    - 实体审计的SQLSugar实现?
    - 测试UowActionFilter,默认是有的?