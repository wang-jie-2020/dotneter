using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Hugin.Mvc.ModelBinding.Metadata
{
    public class HuginModelMetadataProvider : AbpModelMetadataProvider
    {
        public HuginModelMetadataProvider(ICompositeMetadataDetailsProvider detailsProvider)
            : base(detailsProvider)
        {
        }

        public HuginModelMetadataProvider(ICompositeMetadataDetailsProvider detailsProvider, IOptions<MvcOptions> optionsAccessor)
            : base(detailsProvider, optionsAccessor)
        {
        }

        protected override void NormalizeValidationAttrbute(ValidationAttribute validationAttribute)
        {
            base.NormalizeValidationAttrbute(validationAttribute);
        }
    }
}
