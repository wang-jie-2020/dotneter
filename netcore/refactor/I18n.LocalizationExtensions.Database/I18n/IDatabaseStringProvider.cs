namespace I18n.LocalizationExtensions.Database.I18n;

public interface IDatabaseStringProvider
{
    Dictionary<string, string> GetStrings(string cultureName);
}