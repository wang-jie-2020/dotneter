using System;
using System.Collections.Generic;

namespace Hugin.Sms
{
    public class SmsOptions
    {
        public SmsOptions()
        {
            Version = "1.0";
            Extensions = new List<ISmsOptionsExtension>();
        }

        public string Version { get; set; }

        internal IList<ISmsOptionsExtension> Extensions { get; }

        public void RegisterExtension(ISmsOptionsExtension extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException(nameof(extension));
            }

            Extensions.Add(extension);
        }
    }
}
