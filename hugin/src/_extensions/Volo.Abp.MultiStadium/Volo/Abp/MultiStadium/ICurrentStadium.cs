using System;
using JetBrains.Annotations;

namespace Volo.Abp.MultiStadium
{
    public interface ICurrentStadium
    {
        [CanBeNull]
        Guid? Id { get; }

        [CanBeNull]
        string Name { get; }

        IDisposable Change(Guid? id, string name = null);
    }
}
