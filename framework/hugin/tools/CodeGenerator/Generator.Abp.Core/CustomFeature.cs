using System;
using System.Collections.Generic;
using System.IO;

namespace Generator
{
    public class CustomFeature
    {
        public static List<CustomCodeTemplate> GetTemplates()
        {
            if (Global.Features.V1)
            {
                return CustomCodeTemplate.CreateCodeTemplate(Path.Combine(Directory.GetParent(typeof(CustomFeature).Assembly.Location).FullName,
                                                            "CodeTemplates",
                                                            "Server",
                                                            "V1"));
            }

            return new List<CustomCodeTemplate>();
        }

        public static string GetLocator(CustomCodeTemplate template, CustomCode customCode, string fullName)
        {
            var dir = Path.GetDirectoryName(fullName);
            if (dir == null)
            {
                throw new Exception($"{fullName} is not proper");
            }

            if (Global.Features.V1)
            {
                switch (template.Type)
                {
                    case CustomCodeTemplateType.EditDto:
                        fullName = Path.Combine(dir, customCode.EntityName + "_EditDto.cs");
                        break;
                    case CustomCodeTemplateType.ListDto:
                        fullName = Path.Combine(dir, customCode.EntityName + "_ListDto.cs");
                        break;
                    case CustomCodeTemplateType.QueryInput:
                        fullName = Path.Combine(dir, customCode.EntityName + "_QueryInput.cs");
                        break;
                    case CustomCodeTemplateType.Permission:
                        fullName = Path.Combine(dir, customCode.EntityName + "Permissions.cs");
                        break;
                    case CustomCodeTemplateType.GroupPermission:
                        fullName = Path.Combine(dir, "GroupPermissions.cs");
                        break;
                    case CustomCodeTemplateType.GroupErrCode:
                        fullName = Path.Combine(dir, customCode.GroupName + "ErrCodes.cs");
                        break;
                }
            }

            return fullName;
        }
    }
}
