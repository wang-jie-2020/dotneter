using System;
using JetBrains.Annotations;

namespace Volo.Abp.MultiStadium
{
    [Serializable]
    public class StadiumConfiguration
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public StadiumConfiguration()
        {

        }

        public StadiumConfiguration(Guid id, [NotNull] string name)
        {
            Check.NotNull(name, nameof(name));

            Id = id;
            Name = name;
        }
    }
}
