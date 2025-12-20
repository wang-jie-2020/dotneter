using Microsoft.Extensions.Localization;

namespace I18n.LocalizationExtensions;

public class JsonLocalizationOptions : LocalizationOptions
{
    public ResourcesType ResourcesType { get; set; } = ResourcesType.TypeBased;
}