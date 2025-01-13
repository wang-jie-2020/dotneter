using System.IO;
using System.Reflection;

namespace JsonLocalizationExtensions.Internal;

public static class PathHelpers
{
    public static string GetApplicationRoot() => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
}