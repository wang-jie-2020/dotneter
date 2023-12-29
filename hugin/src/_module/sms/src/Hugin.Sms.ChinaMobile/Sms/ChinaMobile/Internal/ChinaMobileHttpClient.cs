using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hugin.Sms.ChinaMobile.Internal.Model;
using Newtonsoft.Json;

namespace Hugin.Sms.ChinaMobile.Internal
{
    /// <summary>
    /// 基于http文档
    /// </summary>
    internal class ChinaMobileHttpClient
    {
        public ChinaMobileHttpClient()
        {
        }

        public async Task<ChinaMobileHttpRsp> SendNormalSms(ChinaMobileHttpSmsNormal input)
        {
            var dic = new Dictionary<string, string>
            {
                {"ecName", input.EcName},
                {"apId", input.ApId},
                {"secretKey",input.SecretKey},
                {"mobiles", string.Join(",", input.Mobiles)},
                {"content", input.Content},
                {"sign", input.Sign},
                {"addSerial", input.AddSerial}
            };

            var sb = new StringBuilder();
            sb.Append(dic["ecName"]);
            sb.Append(dic["apId"]);
            sb.Append(dic["secretKey"]);
            sb.Append(dic["mobiles"]);
            sb.Append(dic["content"]);
            sb.Append(dic["sign"]);
            sb.Append(dic["addSerial"]);
            dic.Add("mac", Md5(sb.ToString()));

            var data = JsonConvert.SerializeObject(dic);
            var encodeData = Base64(data);

            var result = await ApiCallerHelper.Execute<ChinaMobileHttpRsp>(input.Url, HttpMethod.Post, encodeData);
            return result;
        }

        public async Task<ChinaMobileHttpRsp> SendTemplateSms(ChinaMobileHttpSmsTemplate input)
        {
            var dic = new Dictionary<string, string>
            {
                {"ecName", input.EcName},
                {"apId", input.ApId},
                {"secretKey",input.SecretKey},
                {"templateId",input.TemplateId},
                {"mobiles", string.Join(",", input.Mobiles)},
                {"params", JsonConvert.SerializeObject(input.Params)},
                {"sign", input.Sign},
                {"addSerial", input.AddSerial}
            };

            var sb = new StringBuilder();
            sb.Append(dic["ecName"]);
            sb.Append(dic["apId"]);
            sb.Append(dic["secretKey"]);
            sb.Append(dic["templateId"]);
            sb.Append(dic["mobiles"]);
            sb.Append(dic["params"]);
            sb.Append(dic["sign"]);
            sb.Append(dic["addSerial"]);
            dic.Add("mac", Md5(sb.ToString()));

            var data = JsonConvert.SerializeObject(dic);
            var encodeData = Base64(data);

            var result = await ApiCallerHelper.Execute<ChinaMobileHttpRsp>(input.Url, HttpMethod.Post, encodeData);
            return result;
        }

        ///<summary>
        /// 字符串MD5加密
        ///</summary>
        ///<param name="str">要加密的字符串</param>
        ///<param name="charset">编码方式</param>
        ///<returns>密文</returns>
        private string Md5(string str, string charset = "UTF-8")
        {
            var buffer = Encoding.GetEncoding(charset).GetBytes(str);
            try
            {
                var check = new System.Security.Cryptography.MD5CryptoServiceProvider();
                var hashBuffer = check.ComputeHash(buffer);
                var ret = "";
                foreach (var a in hashBuffer)
                    if (a < 16)
                        ret += "0" + a.ToString("X");
                    else
                        ret += a.ToString("X");
                return ret.ToLower();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// base64编码
        /// </summary>
        /// <param name="str">内容</param>
        /// <param name="charset">编码方式</param>
        /// <returns></returns>
        private string Base64(string str, string charset = "UTF-8")
        {
            return Convert.ToBase64String(Encoding.GetEncoding(charset).GetBytes(str));
        }
    }
}
