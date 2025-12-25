// using System;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.Extensions.Options;
//
// namespace I18n.LocaleExtensions
// {
//     public static class ApplicationBuilderExtensions
//     {
//         public static IApplicationBuilder UseLocale(this IApplicationBuilder app)
//         {
//             return app.UseMiddleware<LocaleMiddleware>();
//         }
//         
//         public static IApplicationBuilder UseLocale(this IApplicationBuilder app, LocaleOptions options)
//         {
//             return app.UseMiddleware<LocaleMiddleware>(Options.Create(options));
//         }
//         
//         public static IApplicationBuilder UseLocale(this IApplicationBuilder app, Action<LocaleOptions> optionsAction)
//         {
//             var options = new LocaleOptions();
//             optionsAction.Invoke(options);
//
//             return app.UseMiddleware<LocaleMiddleware>(Options.Create(options));
//         }
//     }
// }