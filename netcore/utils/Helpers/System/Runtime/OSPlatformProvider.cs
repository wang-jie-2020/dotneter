using System.Runtime.InteropServices;

namespace Helpers.System.Runtime;

public class OSPlatformProvider : IOSPlatformProvider
{
    public virtual OSPlatform GetCurrentOSPlatform()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return OSPlatform.OSX; //MAC
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return OSPlatform.Windows;
        }

        return OSPlatform.Linux;
    }
}
