using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Demo.Core
{
    public class CoreOptions
    {
        public CoreOptions()
        {
            Extensions = new List<ICoreOptionsExtension>();
        }

        [AllowNull]
        public string Message { get; set; }

        public List<ICoreOptionsExtension> Extensions { get; }

        public void RegisterExtension(ICoreOptionsExtension extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException(nameof(extension));
            }

            Extensions.Add(extension);
        }
    }
}
