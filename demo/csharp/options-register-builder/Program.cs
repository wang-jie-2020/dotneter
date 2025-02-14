using System;
using System.Linq;
using System.Reflection;
using Demo.Core;
using Demo.Derive;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("请输入命令：0; 退出程序，功能命令：1 - n");
                string input = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrEmpty(input))
                {
                    continue;
                }

                if (input == "0")
                {
                    break;
                }

                Type? type = MethodBase.GetCurrentMethod()?.DeclaringType;
                if (type != null)
                {
                    var method = type.GetMethod("Method" + input,
                        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod);

                    method?.Invoke(null, null);
                }
            }
        }

        /// <summary>
        /// 理解MSDN中的重点句：
        ///     IOptionsFactory<TOptions> 负责新建选项实例。 它具有单个 Create 方法。
        ///     默认实现采用所有已注册 IConfigureOptions<TOptions> 和 IPostConfigureOptions<TOptions> 并首先运行所有配置，然后才进行后期配置。
        ///     它区分 IConfigureNamedOptions<TOptions> 和 IConfigureOptions<TOptions> 且仅调用适当的接口。
        /// </summary>
        static void Method1()
        {
            IServiceCollection services = new ServiceCollection();

            /*
             *  较常见的方式 Configure<TOptions>
             *  Configure<TOptions>
             *      实际是AddSingleton<IConfigureOptions<TOptions>>(new ConfigureNamedOptions<TOptions>(name, configureOptions)); 
             */
            services.Configure<OriginalOptions>(options =>
            {
                options.Name = "Configure";
                options.Value = "Configure";
                options.Extensions.Add("Configure-Emit");
            });

            /*
             *  还有一种方式是ConfigureOptions<TConfigureOptions>
             *      其实就是对AddSingleton<IConfigureOptions<OriginalOptions>, ConfigureOriginalOption>()的封装
             *      但它为什么要注册为Transaction?
             */
            //services.AddSingleton<IConfigureOptions<OriginalOptions>, OriginalOptionsConfigure>();
            services.ConfigureOptions<OriginalOptionsConfigure>();

            /*
             * 甚至可以考虑直接注册Options组件，这种多出现在工厂或者建造者模式下
             *   与Options不同的是，这是可以直接在构造函数注入的，而不是增加泛型包装
             *    场景不一致，没什么可比性
             */
            services.AddTransient(provider =>
            {
                var options = new OriginalOptions()
                {
                    Name = "Direct",
                };
                return options;
            });

            services.AddTransient(provider =>
            {
                var options = new OriginalOptions()
                {
                    Value = "Override"
                };
                return options;
            });

            using var scope = services.BuildServiceProvider();
            {
                {
                    var component = scope.GetServices<IConfigureOptions<OriginalOptions>>();
                    Console.WriteLine($"OriginalOptions的组件有{component.Count()}个");

                    var options = scope.GetRequiredService<IOptions<OriginalOptions>>();
                    Console.WriteLine($"Name:{options.Value.Name}");
                    Console.WriteLine($"Value:{options.Value.Value}");
                    Console.WriteLine($"Extension:{string.Join(',', options.Value.Extensions)}");
                }

                {
                    var component = scope.GetServices<OriginalOptions>();
                    Console.WriteLine($"OriginalOptions的组件有{component.Count()}个");

                    var options = scope.GetRequiredService<OriginalOptions>();
                    Console.WriteLine($"Name:{options.Name}");
                    Console.WriteLine($"Value:{options.Value}");
                    Console.WriteLine($"Extension:{string.Join(',', options.Extensions)}");
                }
            }
        }

        /// <summary>
        /// 通常组件的范围包括服务和配置，前者通常由组件自行封装但后者必然暴露出来，写法千奇百怪，各种Action、Func齐飞
        ///     仿照cap、ef的部分试一试，这里的配置仍旧是IOptions<TOption>
        /// </summary>
        static void Method2()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddCore(options =>
            {
                options.Message = "1";
                options.UseDerive(derive => derive.Message = "2");
            });

            using var scope = services.BuildServiceProvider();
            {
                var coreOptions = scope.GetRequiredService<IOptions<CoreOptions>>();
                Console.WriteLine(coreOptions.Value.Message);

                var deriveOptions = scope.GetRequiredService<IOptions<DeriveOptions>>();
                Console.WriteLine(deriveOptions.Value.Message);
            }
        }
    }
}