using System;
using System.Collections.Generic;
using System.Text;

namespace Sms
{
    public class SmsRsp
    {
        public bool Success { get; set; }

        public string RspCode { get; set; }

        public string RspMsg { get; set; }
    }

    public class SmsRsp<T> : SmsRsp
    {
        public List<T> Data { get; set; }
    }
}
