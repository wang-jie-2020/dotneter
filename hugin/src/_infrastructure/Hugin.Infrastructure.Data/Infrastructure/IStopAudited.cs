using System;

namespace Hugin.Infrastructure
{
    public interface IStopAudited
    {
        bool IsStop { get; set; }

        Guid? StopUserId { get; set; }

        DateTime? StopTime { get; set; }
    }
}
