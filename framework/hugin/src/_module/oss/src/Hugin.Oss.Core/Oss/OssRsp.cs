using System.Collections.Generic;

namespace Hugin.Oss
{
    public class OssRsp
    {
        public bool Success { get; set; }

        public string RspCode { get; set; }

        public string RspMsg { get; set; }
    }

    public class OssRsp<T> : OssRsp
    {
        public List<T> Data { get; set; }
    }
}
