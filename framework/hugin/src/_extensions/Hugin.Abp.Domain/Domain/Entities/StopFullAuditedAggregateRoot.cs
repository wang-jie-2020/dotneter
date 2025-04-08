using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Hugin.Domain.Entities
{
    public abstract class StopFullAuditedAggregateRoot<TKey> : FullAuditedAggregateRoot<TKey>, IStopAudited
    {
        public virtual bool IsStop { get; set; }

        public virtual Guid? StopUserId { get; set; }

        public virtual DateTime? StopTime { get; set; }
    }

    public abstract class StopFullAuditedAggregateRoot : FullAuditedAggregateRoot, IStopAudited
    {
        public virtual bool IsStop { get; set; }

        public virtual Guid? StopUserId { get; set; }

        public virtual DateTime? StopTime { get; set; }
    }
}
