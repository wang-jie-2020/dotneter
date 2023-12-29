using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Hugin.Sms.Aliyun.Internal;
using Hugin.Sms.Aliyun.Internal.Model;
using Microsoft.Extensions.Options;

namespace Hugin.Sms.Aliyun
{
    public class AliyunSmsService : ISmsService, IAliSmsService
    {
        private readonly AliyunSmsClient _client;
        private readonly AliyunSmsOptions _options;

        public AliyunSmsService(IOptions<AliyunSmsOptions> options)
        {
            _options = options.Value;
            _client = new AliyunSmsClient(options.Value.AccessKeyId, options.Value.AccessKeySecret);
        }

        public Task<SmsRsp> SendNormalSms(string[] mobiles, string content, string sign = "")
        {
            throw new NotImplementedException();
        }

        public async Task<SmsRsp> SendTemplateSms(string[] mobiles, string templateId,
            Dictionary<string, string> parameters, string sign = "")
        {
            var result = new SmsRsp();

            try
            {
                var input = new AliyunSmsTemplate
                {
                    TemplateCode = templateId,
                    Mobiles = mobiles,
                    Params = parameters,
                    SignName = string.IsNullOrWhiteSpace(sign) ? _options.Sign : sign
                };

                var rsp = await _client.SendTemplateSms(input);

                if ("OK".Equals(rsp.Code, StringComparison.CurrentCultureIgnoreCase))
                {
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.RspMsg = rsp.Message;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.RspCode = string.Empty;
                result.RspMsg = ex.Message;
            }

            return result;
        }

        public async Task<SmsRsp<QuerySendDetailsResponse.QuerySendDetails_SmsSendDetailDTO>> QuerySendDetails(AliyunSmsQuerySendDetailsInput input)
        {
            var result = new SmsRsp<QuerySendDetailsResponse.QuerySendDetails_SmsSendDetailDTO>();

            try
            {
                var rsp = await _client.QuerySendDetails(input);

                if ("OK".Equals(rsp.Code, StringComparison.CurrentCultureIgnoreCase))
                {
                    result.Success = true;
                    result.Data = rsp.SmsSendDetailDTOs;
                }
                else
                {
                    result.Success = false;
                    result.RspMsg = rsp.Message;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.RspCode = string.Empty;
                result.RspMsg = ex.Message;
            }

            return result;
        }
    }
}
