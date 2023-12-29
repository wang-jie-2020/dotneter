using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hugin.Sms
{
    public interface ISmsService
    {
        /// <summary>
        /// 普通短信
        /// </summary>
        /// <param name="mobiles">手机号码</param>
        /// <param name="content">内容</param>
        /// <param name="sign">签名</param>
        /// <returns></returns>
        Task<SmsRsp> SendNormalSms(string[] mobiles, string content, string sign = "");

        /// <summary>
        /// 模板短信
        /// </summary>
        /// <param name="mobiles">手机号码</param>
        /// <param name="templateId">模板Id</param>
        /// <param name="parameters">模板参数</param>
        /// <param name="sign">签名</param>
        /// <returns></returns>
        Task<SmsRsp> SendTemplateSms(string[] mobiles, string templateId, Dictionary<string, string> parameters, string sign = "");
    }
}
