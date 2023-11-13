#if UNITY

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LibVLCSharp
{
    public static partial class Core
    {
        partial struct Native
        {
            [DllImport(Constants.UnityPlugin)]
            internal static extern void SetPluginPath(string path);

            [DllImport(Constants.UnityPlugin)]
            internal static extern void Print(string toPrint);

            [DllImport("api-ms-win-core-libraryloader-l2-1-0.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern IntPtr LoadPackagedLibrary(string dllToLoad, uint reserved = 0);
        }

        /// <summary>
        /// Initializes libvlc for libvlcsharp in a Unity context. By using runtime checks, we can have the same libvlcsharp
        /// netstandard dll running on all Unity platforms, which in turn simplifies usage from the Unity Editor when debugging
        /// actively on multiple platforms.
        /// </summary>
        /// <param name="libvlcDirectoryPath">Path to load libvlc from</param>
        /// <exception cref="VLCException"></exception>
        internal static void InitializeUnity(string? libvlcDirectoryPath = null)
        {
            // only VLC for Unity on Windows and MacOS currently requires pre-initialization logic
            if(!PlatformHelper.IsWindows && !PlatformHelper.IsMac)
              return;

            if(string.IsNullOrEmpty(libvlcDirectoryPath))
            {
                throw new VLCException("Please provide UnityEngine.Application.dataPath to Core.Initialize for proper initialization.");
            }
          
            LibvlcHandle = Native.LoadPackagedLibrary(Constants.LibraryName);
            if (LibvlcHandle == IntPtr.Zero)
            {
                throw new VLCException($"Failed to load {Constants.LibraryName}{Constants.WindowsLibraryExtension}, error {Marshal.GetLastWin32Error()}." +
                    $"Please make sure that this library, {Constants.CoreLibraryName}{Constants.WindowsLibraryExtension} and the plugins are copied to the `AppX` folder." +
                    "For that, you can reference the `VideoLAN.LibVLC.UWP` NuGet package.");
            }
        }

        // from https://github.com/qmatteoq/DesktopBridgeHelpers

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder packageFullName);

        static bool IsUWP()
        {
            if (IsWindows7OrLower)
            {
                return false;
            }
            else
            {
                var length = 0;
                var sb = new StringBuilder(0);
                GetCurrentPackageFullName(ref length, sb);

                sb = new StringBuilder(length);
                return GetCurrentPackageFullName(ref length, sb) == 0;
            }
        }

        static bool IsWindows7OrLower
        {
            get
            {
                var versionMajor = Environment.OSVersion.Version.Major;
                var versionMinor = Environment.OSVersion.Version.Minor;
                var version = versionMajor + (double)versionMinor / 10;
                return version <= 6.1;
            }
        }
    }
}
#endif
