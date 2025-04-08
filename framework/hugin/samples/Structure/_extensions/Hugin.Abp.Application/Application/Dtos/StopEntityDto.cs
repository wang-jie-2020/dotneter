using Hugin.Domain.Entities;
using Volo.Abp.Application.Dtos;

namespace Hugin.Application.Dtos
{
    public abstract class StopEntityDto<TKey> : EntityDto<TKey>, IStop
    {
        public virtual bool IsStop { get; set; }
    }

    public abstract class StopEntityDto : EntityDto, IStop
    {
        public virtual bool IsStop { get; set; }
    }
}
