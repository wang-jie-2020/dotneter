namespace Yi.AspNetCore.I18n;

public interface IDatabaseStringProvider
{
    Dictionary<string, string> GetStrings(string cultureName);
}