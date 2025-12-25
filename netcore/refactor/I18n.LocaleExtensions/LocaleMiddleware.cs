// using System;
// using System.Globalization;
// using System.Threading;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Options;
//
// namespace I18n.LocaleExtensions
// {
//     public class LocaleMiddleware
//     {
//         private readonly RequestDelegate _next;
//         private readonly LocaleOptions _options;
//
//         public LocaleMiddleware(RequestDelegate next, IOptions<LocaleOptions> options)
//         {
//             if (options == null)
//             {
//                 throw new ArgumentNullException(nameof(options));
//             }
//
//             _next = next ?? throw new ArgumentNullException(nameof(next));
//             _options = options.Value;
//         }
//         
//         public async Task InvokeAsync(HttpContext context)
//         {
//             SetCurrentLocale();
//             await _next(context);
//         }
//
//         private void SetCurrentLocale()
//         {
//             
//         }
//         
//         public Locale Current {
//             get => _currentLocale.Value;
//             set => _currentLocale.Value = value;
//         }
//         
//         private readonly AsyncLocal<Locale> _currentLocale = new AsyncLocal<Locale>();
//     }
// }