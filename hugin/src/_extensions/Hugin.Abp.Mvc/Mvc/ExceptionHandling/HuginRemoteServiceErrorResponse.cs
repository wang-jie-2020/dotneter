using System;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Http;
using Volo.Abp.Validation;

namespace Hugin.Mvc.ExceptionHandling
{
    public class HuginRemoteServiceErrorResponse : RemoteServiceErrorResponse
    {
        public string Success => "false";

        public string Code { get; set; }

        public HuginRemoteServiceErrorResponse(Exception exception, RemoteServiceErrorInfo error) : base(error)
        {
            Code = GetDescriptionCode(exception);
        }

        protected string GetDescriptionCode(Exception exception)
        {
            if (IsLocalBusinessException(exception))
            {
                return "1";
            }

            return "2";
        }

        public virtual bool IsLocalBusinessException(Exception exception)
        {
            if (exception is AbpValidationException ||
                exception is AbpAuthorizationException ||
                exception is EntityNotFoundException ||
                exception is IBusinessException)
            {
                return true;
            }

            if (exception is AggregateException aggException && aggException.InnerException != null)
            {
                return IsLocalBusinessException(aggException.InnerException);
            }

            return false;
        }
    }
}









