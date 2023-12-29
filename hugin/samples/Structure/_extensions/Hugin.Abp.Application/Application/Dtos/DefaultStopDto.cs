using System;
using Volo.Abp.Auditing;

namespace Hugin.Application.Dtos
{
    public class DefaultStopDto<TKey> : StopEntityDto<TKey>, IHasCreationTime
    {
        public DateTime CreationTime { get; set; }
    }
}
