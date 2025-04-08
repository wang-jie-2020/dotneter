using Hugin.Domain.Entities;
using System;
using Volo.Abp.Application.Dtos;

namespace Hugin.Application.Dtos
{
    public abstract class StopFullAuditEntityDto<TKey> : FullAuditedEntityDto<TKey>, IStopAudited
    {
        public virtual bool IsStop { get; set; }

        public virtual Guid? StopUserId { get; set; }

        public virtual DateTime? StopTime { get; set; }
    }

    public abstract class StopFullAuditEntityDto : FullAuditedEntityDto, IStopAudited
    {
        public virtual bool IsStop { get; set; }

        public virtual Guid? StopUserId { get; set; }

        public virtual DateTime? StopTime { get; set; }
    }
}
