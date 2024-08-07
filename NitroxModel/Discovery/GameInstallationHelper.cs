using System.IO;
using System.Runtime.InteropServices;

namespace NitroxModel.Discovery;

public static class GameInstallationHelper
{
    public static bool HasGameExecutable(string path, GameInfo gameInfo)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return false;
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return File.Exists(Path.Combine(path, "MacOS", gameInfo.ExeName));
        }
        return File.Exists(Path.Combine(path, gameInfo.ExeName));
    }

    public static bool HasValidGameFolder(string path, GameInfo gameInfo)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return false;
        }
        if (!Directory.Exists(path))
        {
            return false;
        }
        if (!HasGameExecutable(path, gameInfo))
        {
            return false;
        }
        if (!Directory.Exists(Path.Combine(path, gameInfo.DataFolder, "Managed")))
        {
            return false;
        }

        return true;
    }
}
