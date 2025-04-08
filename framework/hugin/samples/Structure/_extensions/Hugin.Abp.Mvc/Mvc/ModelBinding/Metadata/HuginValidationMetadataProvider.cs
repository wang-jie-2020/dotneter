using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Volo.Abp.AspNetCore.Mvc.Validation;

namespace Hugin.Mvc.ModelBinding.Metadata
{
    /*
     *  我认为aspnetcore团队设计这块时，是推荐Mvc.Options.ModelMetadataDetailsProviders.Add(IValidationMetadataProvider myProvider)
     *  而abp则扩展了DefaultModelMetadataProvider，这不是严谨的做法
     */
    public class HuginValidationMetadataProvider : IValidationMetadataProvider
    {
        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            foreach (var attribute in context.ValidationMetadata.ValidatorMetadata)
            {
                if (attribute is ValidationAttribute validationAttribute && validationAttribute.ErrorMessage == null)
                {
                    ValidationAttributeHelper.SetDefaultErrorMessage(validationAttribute);
                }
            }
        }
    }
}
