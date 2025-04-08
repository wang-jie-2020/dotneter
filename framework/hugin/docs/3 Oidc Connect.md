# Oidc Connect

![img](images\168328-20170530092134102-504300351.jpg)

针对NetCore，最佳选择当然是IdentityServer4 + Identity ，这两者的基础文档请msdn，这里只讨论对其进行的一些修改和设计。

## Identity

ABP已经集成好AbpUser和AbpRole等，对它们的重写参见实体模型一节。这里只要讨论关于Identity的配置和约定。

### IdentityOptions

通常对组件的配置可以这么写：

```csharp
Configure<IdentityOptions>(options =>
                           {
                               options.Password.RequireDigit = false;
                               options.Password.RequireLowercase = false;
                               options.Password.RequireUppercase = false;
                               options.Password.RequiredUniqueChars = 0;
                               options.Password.RequireNonAlphanumeric = false;
                           });
```

但ABP集成了`IdentityOptions`与配置模块，见Volo.Abp.Identity.AbpIdentityOptionsManager

其设计目的大概是在于针对不同的租户提供不同的配置选项，目前不考虑到这么复杂，在数据种子中整体修改。

```csharp
    public class IdentityOptionsDataSeeder : IDataSeedContributor, ITransientDependency
    {
        private readonly ISettingManager _settingManager;

        public IdentityOptionsDataSeeder(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await _settingManager.SetGlobalAsync(IdentitySettingNames.Password.RequireDigit, false.ToString());
            await _settingManager.SetGlobalAsync(IdentitySettingNames.Password.RequireLowercase, false.ToString());
            await _settingManager.SetGlobalAsync(IdentitySettingNames.Password.RequireUppercase, false.ToString());
            await _settingManager.SetGlobalAsync(IdentitySettingNames.Password.RequireNonAlphanumeric, false.ToString());
            await _settingManager.SetGlobalAsync(IdentitySettingNames.Password.RequiredUniqueChars, 0.ToString());
        }
    }
```

### 约定

1. 总是由认证模块管理认证数据
2. 管理员与C端用户都是一套体系
3. C端用户由客户端请求到认证模块建立

## IdentityServer 4

主要考虑的是授权码模式与资源管理者账户密码模式，即`Code`和`Password`模式，当然混合或客户端模式也支持。

### 证书

- 证书生成

  ```bash
  openssl req -newkey rsa:2048 -nodes -keyout ids4.key -x509 -days 36500 -out ids4.cer
  openssl pkcs12 -export -in ids4.cer -inkey ids4.key -out ids4.pfx
  ```

- 证书注册

  证书是作为资源放在dll中，当然也可以直接物理文件

  ```csharp
   PreConfigure<AbpIdentityServerBuilderOptions>(options =>
              {
                  options.AddDeveloperSigningCredential = false;
              });
  
              PreConfigure<IIdentityServerBuilder>(builder =>
              {
                  //Certificate
                  var certificate = Assembly.GetEntryAssembly().GetManifestResourceStream(typeof(SampleIdentityServerModule).Namespace + ".ids4.pfx");
                  if (certificate == null)
                  {
                      throw new FileNotFoundException("certificate missing");
                  }
  
                  var buffer = new byte[certificate.Length];
                  certificate.Read(buffer, 0, buffer.Length);
  
                  builder.AddSigningCredential(new X509Certificate2(buffer, "lg"));
              });
  ```

### 安全

主要是

- CSRF，默认集成的`ValidateAntiforgery`意义不足，还影响部分API，模块统一去掉。

- Cookie策略问题（针对NetCore的Razor或Mvc）

  ```csharp
          app.UseCookiePolicy(new CookiePolicyOptions
          {
              /*  
               *  SameSitePolicy = None --> http request error
               *  see https://docs.microsoft.com/zh-cn/aspnet/core/security/samesite?view=aspnetcore-5.0
               */
              MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax
          });
  ```

### Jwt

#### UserClaim

在Jwt中的claim中增加，示例微信Id

    public class LGClaimsService : AbpClaimsService
    {
        public LGClaimsService(IProfileService profile, ILogger<DefaultClaimsService> logger) : base(profile, logger)
        {
        }
    
        protected override IEnumerable<string> FilterRequestedClaimTypes(IEnumerable<string> claimTypes)
        {
            return base.FilterRequestedClaimTypes(claimTypes).Union(new[] { "wechat"});
        }
    }
    public class LGUserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory
    {
        public LGUserClaimsPrincipalFactory(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> options,
            ICurrentPrincipalAccessor currentPrincipalAccessor,
            IAbpClaimsPrincipalFactory abpClaimsPrincipalFactory)
            : base(userManager, roleManager, options, currentPrincipalAccessor, abpClaimsPrincipalFactory)
        {
        }
    
        public override async Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
        {
            var principal = await base.CreateAsync(user);
            var identity = principal.Identities.First();
    
            if (!user.GetUserOpenId().IsNullOrWhiteSpace())
            {
                identity.AddIfNotContains(new Claim("wechat", user.GetUserOpenId()));
            }
    
            return principal;
        }
    }

