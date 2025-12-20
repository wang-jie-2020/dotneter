using System;
using System.Linq.Expressions;
using Microsoft.Extensions.Localization;

namespace I18n.LocalizationExtensions;

public static class StringLocalizerExtensions
{
    public static LocalizedString GetString<TResource>(
        this IStringLocalizer stringLocalizer,
        Expression<Func<TResource, string>> propertyExpression) => stringLocalizer[(propertyExpression.Body as MemberExpression).Member.Name];
}