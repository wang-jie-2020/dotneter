using System.Runtime.InteropServices;

namespace Helpers.System.Runtime;

public interface IOSPlatformProvider
{
    OSPlatform GetCurrentOSPlatform();
}
