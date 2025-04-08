using System;
using System.Collections.Generic;
using System.Text;

namespace Sms.ChinaMobile.Internal.Model
{
    public class ChinaMobileHttpTemplateSms
    {
        /// <summary>
        /// 集团客户名称
        /// </summary>
        public string EcName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string ApId { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 模版ID
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string[] Mobiles { get; set; }

        /// <summary>
        /// 变量
        /// </summary>
        public string[] Params { get; set; }

        /// <summary>
        /// 签名编码
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// 扩展码
        /// 只发短信传空字符串(“”)即可；用来关联上行短信)为精确匹配，此项填写空字符串（""）；
        /// </summary>
        public string AddSerial { get; set; }

        ///// <summary>
        ///// API输入参数签名结果
        ///// 签名算法：将ecName，apId，secretKey，templateId，mobiles，params，sign，addSerial的值按照顺序拼接（中间无符号），然后通过md5(32位小写)计算后得出的值
        ///// </summary>
        //public string Mac { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }
    }
}
