using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sms.Aliyun.Internal.Model;

namespace Sms.Aliyun.Internal
{
    public class AliyunClient
    {
        private readonly string _accessKeyId;
        private readonly string _accessKeySecret;

        private const string RegionId = "cn-hangzhou";
        public const string EndpointName = "cn-hangzhou";
        public const string Domain = "dysmsapi.aliyuncs.com";
        public const string Product = "Dysmsapi";

        public AliyunClient(string accessKeyId, string accessKeySecret)
        {
            _accessKeyId = accessKeyId;
            _accessKeySecret = accessKeySecret;
        }

        private IAcsClient _acsClient;

        public IAcsClient AcsClient
        {
            get
            {
                if (_acsClient == null)
                {
                    var profile = DefaultProfile.GetProfile(RegionId, _accessKeyId, _accessKeySecret);
                    profile.AddEndpoint(EndpointName, RegionId, Product, Domain);
                    _acsClient = new DefaultAcsClient(profile);
                }

                return _acsClient;
            }
        }

        public async Task<SendSmsResponse> SendTemplateSms(AliyunSmsTemplate input)
        {
            var request = new SendSmsRequest()
            {
                PhoneNumbers = string.Join(",", input.Mobiles),
                SignName = input.SignName,
                TemplateCode = input.TemplateCode,
                TemplateParam = JsonConvert.SerializeObject(input.Params),
            };

            var rsp = AcsClient.GetAcsResponse(request);
            return await Task.FromResult(rsp);
        }

        /// <summary>
        /// 查看短信发送记录和发送状态。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<QuerySendDetailsResponse> QuerySendDetails(AliyunSmsQuerySendDetailsInput input)
        {
            var request = new QuerySendDetailsRequest
            {
                PhoneNumber = input.Mobile,
                SendDate = input.SendDate.ToString("yyyyMMDD"),
                PageSize = input.PageSize,
                CurrentPage = input.CurrentPage
            };

            var rsp = AcsClient.GetAcsResponse(request);
            return Task.FromResult(rsp);
        }
    }
}
