using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.WebApi.Server
{
    public class ServiceInfo
    {
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public int GrpcPort { get; set; }
    }
}
