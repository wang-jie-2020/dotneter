using System;

namespace Hugin.Domain.Entities
{
    public interface IStopAudited : IStop
    {
        Guid? StopUserId { get; set; }

        DateTime? StopTime { get; set; }
    }
}
