using System;
using JetBrains.Annotations;

namespace Volo.Abp.MultiStadium
{
    public class BasicStadiumInfo
    {
        [CanBeNull]
        public Guid? StadiumId { get; }

        [CanBeNull]
        public string Name { get; }

        public BasicStadiumInfo(Guid? stadiumId, string name = null)
        {
            StadiumId = stadiumId;
            Name = name;
        }
    }
}