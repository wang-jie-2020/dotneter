using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AESC.Utils.Common;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;

namespace AESC.Utils.AbpExtensions
{
    public static class EntityExtensions
    {
        public static void SetId(this IEntity<long> entity)
        {
            if (entity.Id == 0)
            {
                var propertyInfo = entity.GetType().GetProperty(nameof(entity.Id));

                if (propertyInfo == null || propertyInfo.GetSetMethod(true) == null)
                {
                    return;
                }

                propertyInfo.SetValue(entity, IdGen.Next());
            }
        }
    }
}
