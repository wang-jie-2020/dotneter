using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Consul
{
    public class ConsulService
    {
        public ConsulService(string id, string name, string host, int port, IEnumerable<string> tags) =>
            (Id, Name, Host, Port, Tags) = (id, name, host, port, tags);

        public string Id { get; }
        public string Name { get; }
        public string Host { get; }
        public int Port { get; }
        public IEnumerable<string> Tags { get; }

    }
}
