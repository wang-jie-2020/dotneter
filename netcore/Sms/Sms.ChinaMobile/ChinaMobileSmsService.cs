using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sms.ChinaMobile.Internal;
using Sms.ChinaMobile.Internal.Model;

namespace Sms.ChinaMobile
{
    public class ChinaMobileSmsService : ISmsService
    {
        private readonly ChinaMobileHttpClient _client;
        private readonly ChinaMobileSmsOptions _options;

        public ChinaMobileSmsService(ChinaMobileHttpClient client, IOptions<ChinaMobileSmsOptions> options)
        {
            _client = client;
            _options = options.Value;
        }

        public async Task<SmsRsp> SendNormalSms(string[] mobiles, string content, string sign = "")
        {
            var result = new SmsRsp();

            try
            {
                var input = new ChinaMobileHttpNormalSms
                {
                    EcName = _options.EcName,
                    ApId = _options.AppId,
                    SecretKey = _options.AppSecret,
                    Mobiles = mobiles,
                    Content = content,
                    Sign = string.IsNullOrWhiteSpace(sign) ? _options.Sign : sign,
                    AddSerial = _options.AddSerial,
                    Url = _options.NorSubmitUrl
                };

                var rsp = await _client.SendNormalSms(input);
                result.Success = rsp.Success;
                result.RspCode = rsp.Rspcod;
                result.RspMsg = EnumHelper.GetEnumDesc<ChinaMobileHttpRspCode>(rsp.Rspcod);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.RspCode = string.Empty;
                result.RspMsg = ex.Message;
            }

            return result;
        }

        public async Task<SmsRsp> SendTemplateSms(string[] mobiles, string templateId, Dictionary<string, string> parameters, string sign = "")
        {
            var result = new SmsRsp();

            try
            {
                var input = new ChinaMobileHttpTemplateSms
                {

                    EcName = _options.EcName,
                    ApId = _options.AppId,
                    SecretKey = _options.AppSecret,
                    Mobiles = mobiles,
                    TemplateId = templateId,
                    Params = parameters.Values.ToArray(),
                    Sign = string.IsNullOrWhiteSpace(sign) ? _options.Sign : sign,
                    AddSerial = _options.AddSerial,
                    Url = _options.TmpSubmitUrl
                };

                var rsp = await _client.SendTemplateSms(input);
                result.Success = rsp.Success;
                result.RspCode = rsp.Rspcod;
                result.RspMsg = EnumHelper.GetEnumDesc<ChinaMobileHttpRspCode>(rsp.Rspcod);
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
