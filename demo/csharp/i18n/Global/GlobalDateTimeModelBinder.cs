using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore.Metadata;

namespace i18n;

public class GlobalDateTimeModelBinder : DateTimeModelBinder
{
    public GlobalDateTimeModelBinder(DateTimeStyles supportedStyles, ILoggerFactory loggerFactory) : base(supportedStyles, loggerFactory)
    {
    }
}