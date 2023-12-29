using System;
using System.Collections.Generic;
using System.Text;

namespace Sms.Aliyun.Internal.Model
{
    public class AliyunSmsTemplate
    {
        /// <summary>
        /// 模板Id
        /// </summary>
        public string TemplateCode { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string[] Mobiles { get; set; }

        ///// <summary>
        ///// 模板参数
        ///// </summary>
        public Dictionary<string, string> Params { get; set; }

        /// <summary>
        /// 短信签名
        /// </summary>
        public string SignName { get; set; }
    }
}
