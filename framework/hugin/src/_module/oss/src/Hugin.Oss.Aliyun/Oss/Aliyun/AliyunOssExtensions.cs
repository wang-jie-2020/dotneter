using System;
using Aliyun.OSS.Model;

namespace Hugin.Oss.Aliyun
{
    public static class AliyunOssExtensions
    {
        public static TResponse HandlerError<TResponse>(this TResponse response, string friendlyMessage = null) where TResponse : GenericResult
        {
            var code = (int)response.HttpStatusCode;
            if (code < 300 || code >= 600)
                return response;

            var message = response.ResponseMetadata["Message"];
            var requestId = response.ResponseMetadata["RequestId"];
            var traceId = response.ResponseMetadata["TraceId"];
            var resource = response.ResponseMetadata["Resource"];
            throw new Exception($"阿里云存储错误,详细信息:RequestId:{requestId},traceId:{traceId},resource:{resource}");
        }
    }
}
