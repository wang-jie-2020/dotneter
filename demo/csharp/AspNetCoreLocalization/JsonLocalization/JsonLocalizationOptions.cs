using Microsoft.Extensions.Localization;

namespace JsonLocalizationExtensions;

public class JsonLocalizationOptions : LocalizationOptions
{
    public ResourcesType ResourcesType { get; set; } = ResourcesType.TypeBased;
}