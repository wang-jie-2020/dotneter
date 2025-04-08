using System.Threading.Tasks;
using Aliyun.Acs.Dysmsapi.Model.V20170525;

namespace Hugin.Sms.Aliyun
{
    public interface IAliSmsService
    {
        /// <summary>
        /// 查看短信发送记录和发送状态。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SmsRsp<QuerySendDetailsResponse.QuerySendDetails_SmsSendDetailDTO>> QuerySendDetails(AliyunSmsQuerySendDetailsInput input);
    }
}
