using System;
using System.Collections.Generic;

namespace Hugin.Oss
{
    public class OssOptions
    {
        public OssOptions()
        {
            Version = "1.0";
            Extensions = new List<IOssOptionsExtension>();
        }

        public string Version { get; set; }

        internal IList<IOssOptionsExtension> Extensions { get; }

        public void RegisterExtension(IOssOptionsExtension extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException(nameof(extension));
            }

            Extensions.Add(extension);
        }
    }
}
