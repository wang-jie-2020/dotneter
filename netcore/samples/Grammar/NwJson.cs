using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Grammar
{
    internal class NwJson
    {
        const string json = @"{
              ""code"": 200,
              ""msg"": null,
              ""data"": {
                ""access_token"": ""xxxxa34befc2026b659d657ae133460a983a7ed703"",
                ""app_type"": ""singleton"",
                ""expires_in"": 43200
              },
              ""success"": true
        }";

        void Method1()
        {
            var jsonObject = JObject.Parse(json);

            var data = jsonObject["data"];

            Console.WriteLine(data.ToString());

            var token = data["access_token"];

            Console.WriteLine(token);

            var data1 = jsonObject["data"]["access_token"];
            Console.WriteLine(data1);
        }
    }
}
