using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace i18n;

public class GlobalBodyModelBinderProvider : BodyModelBinderProvider
{
    public GlobalBodyModelBinderProvider(IList<IInputFormatter> formatters, IHttpRequestStreamReaderFactory readerFactory) : base(formatters, readerFactory)
    {
    }

    public GlobalBodyModelBinderProvider(IList<IInputFormatter> formatters, IHttpRequestStreamReaderFactory readerFactory, ILoggerFactory loggerFactory) : base(formatters, readerFactory, loggerFactory)
    {
    }

    public GlobalBodyModelBinderProvider(IList<IInputFormatter> formatters, IHttpRequestStreamReaderFactory readerFactory, ILoggerFactory loggerFactory, MvcOptions? options) : base(formatters, readerFactory, loggerFactory, options)
    {
    }
}