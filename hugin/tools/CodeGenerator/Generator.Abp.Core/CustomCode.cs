using Generator.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Generator
{
    /// <summary>
    /// FullNameSpace = AssemblyNameSpace + ModuleName + OtherNameSpace
    /// e.g LG.NetCore.Platform.Stadiums = LG.NetCore + Platform + Stadiums
    /// </summary>
    public class CustomCode
    {
        public string FullNameSpace { get; }

        public string AssemblyNameSpace { get; }

        public string ModuleName { get; }

        public string OtherNameSpace { get; }

        public string GroupName { get; }

        public string EntityName { get; }

        public string EntityNameLower => EntityName.Substring(0, 1).ToLower() + EntityName.Substring(1, EntityName.Length - 1);

        public string EntityDisplayName { get; set; }

        public bool HasStop { get; set; }

        public List<CustomEntityProperty> Properties { get; set; } = new List<CustomEntityProperty>();

        public CustomCode(string fullNameSpace, string assemblyNameSpace, string entityName)
        {
            FullNameSpace = fullNameSpace;
            AssemblyNameSpace = assemblyNameSpace;

            var names = FullNameSpace.TrimStart(AssemblyNameSpace).Split(new[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries);
            ModuleName = names[0];
            GroupName = names[names.Length - 1];

            var otherNames = names.Except(new List<string>() { names[0] }).ToArray();
            OtherNameSpace = string.Join(".", otherNames);

            EntityName = entityName;
        }
    }
}
