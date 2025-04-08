using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;

namespace Hugin.Application.Dtos
{
    public class DefaultDto<TKey> : EntityDto<TKey>, IHasCreationTime
    {
        public DateTime CreationTime { get; set; }
    }
}
