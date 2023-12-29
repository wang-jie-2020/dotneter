using System.Collections.Generic;

namespace Custom.Models
{
    public class ConsulConfig
    {
        public ConsulServer Server { get; set; }

        public List<ConsulService> Services { get; set; }
    }

    public class ConsulServer
    {
        public string Ip { get; set; }

        public int Port { get; set; }
    }

    public class ConsulService
    {
        public string Ip { get; set; }

        public int Port { get; set; }

        public string Name { get; set; }
    }
}
