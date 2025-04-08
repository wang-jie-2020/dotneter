using System.Collections.Generic;
using System.Linq;

namespace Generator
{
    public class CustomCode
    {
        public string Location { get; set; }

        public string FullNameSpace { get; }

        public string EntityName { get; }

        public string EntityNameLower => EntityName.Substring(0, 1).ToLower() + EntityName.Substring(1, EntityName.Length - 1);

        public string EntityDisplayName { get; set; }

        public bool HasStop
        {
            get
            {
                return Properties.Any(p => p.Name.Equals("IsStop", System.StringComparison.OrdinalIgnoreCase));
            }
        }

        public bool HasTenant
        {
            get
            {
                return Properties.Any(p => p.Name.Equals("TenantId", System.StringComparison.OrdinalIgnoreCase));
            }
        }

        public bool HasUser
        {
            get
            {
                return Properties.Any(p => p.Name.Equals("UserId", System.StringComparison.OrdinalIgnoreCase));
            }
        }

        public List<CustomEntityProperty> Properties { get; set; } = new List<CustomEntityProperty>();

        public CustomCode(string fullNameSpace, string entityName)
        {
            FullNameSpace = fullNameSpace;
            EntityName = entityName;
        }
    }
}
