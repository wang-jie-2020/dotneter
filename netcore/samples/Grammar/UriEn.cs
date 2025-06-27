using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;

namespace Grammar
{
    internal class UriEn
    {
        const string urlString = @"http://10.206.121.15:32037/upload/ARR台账/DefaultPath/2025/06/26/sadasd265455544.xlsx";
        const string urlString2 = @"http://10.202.33.132:5005/upload/ARR台账/DefaultPath/2025/06/26/6165614613161520250626182253.xlsx";

        void Method1()
        {
            Console.WriteLine("===================================Uri.EscapeUriString");
            Console.WriteLine(Uri.EscapeUriString(urlString));

            Console.WriteLine("===================================Uri.EscapeDataString");
            Console.WriteLine(Uri.EscapeDataString(urlString));

            Console.WriteLine("===================================WebUtility.UrlEncode");
            Console.WriteLine(WebUtility.UrlEncode(urlString));

            Console.WriteLine("===================================HttpUtility.UrlEncode");
            Console.WriteLine(HttpUtility.UrlEncode(urlString));

            //Console.WriteLine(UrlEncoder.Create().Encode(urlString));
        }

        void Method2()
        {
            var url = Uri.EscapeUriString(urlString);
            Console.WriteLine(url);

            var bytes = Encoding.UTF8.GetBytes(url);

            var base64String = Convert.ToBase64String(bytes);
            Console.WriteLine(base64String);

            Console.WriteLine(Uri.EscapeDataString(base64String));
        }
    }
}
