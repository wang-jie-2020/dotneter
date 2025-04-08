using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Sms.Aliyun.Internal.Model;

namespace Sms.Aliyun
{
    public interface IAliyunSmsService
    {
        /// <summary>
        /// 查看短信发送记录和发送状态。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SmsRsp<QuerySendDetailsResponse.QuerySendDetails_SmsSendDetailDTO>> QuerySendDetails(AliyunSmsQuerySendDetailsInput input);
    }
}
