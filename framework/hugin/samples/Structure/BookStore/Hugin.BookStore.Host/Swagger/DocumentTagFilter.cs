//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.SwaggerGen;

//namespace LG.NetCore.Sample.Swagger
//{
//    [Obsolete]
//    public class DocumentTagFilter : IDocumentFilter
//    {
//        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
//        {
//            var tags = new List<OpenApiTag>
//            {
//                new OpenApiTag
//                {
//                    Name = "Sample",
//                    Description = "示例",
//                    ExternalDocs = new OpenApiExternalDocs { Description = "包含：示例/DEMO" }
//                }
//            };

//            swaggerDoc.Tags = tags.OrderBy(x => x.Name).ToList();
//        }
//    }
//}
