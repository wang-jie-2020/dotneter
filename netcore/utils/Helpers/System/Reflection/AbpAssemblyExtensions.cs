using System.Diagnostics;
using System.Reflection;

namespace Helpers.System.Reflection;

public static class AbpAssemblyExtensions
{
    public static string GetFileVersion(this Assembly assembly)
    {
        return FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
    }
}
