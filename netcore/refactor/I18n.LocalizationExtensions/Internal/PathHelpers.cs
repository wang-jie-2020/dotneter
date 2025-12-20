using System.IO;
using System.Reflection;

namespace I18n.LocalizationExtensions.Internal;

public static class PathHelpers
{
    public static string GetApplicationRoot() => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
}