using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LibVLCSharp
{
    /// <summary>
    /// Small helper for determining the current platform
    /// </summary>
    internal class PlatformHelper
    {
        /// <summary>
        /// Returns true if running on Windows, false otherwise
        /// </summary>
        internal static bool IsWindows
        {
#if NET45
            get => Environment.OSVersion.Platform == PlatformID.Win32NT;
#elif UWP
            get => true;
#else
            get => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif
        }

        /// <summary>
        /// Returns true if running on Linux, false otherwise
        /// </summary>
        internal static bool IsLinux
        {
#if NET45
            get => Environment.OSVersion.Platform == PlatformID.Unix;
#elif UWP
            get => false;
#else
            get => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
#endif
        }

        /// <summary>
        /// Returns true if running on Linux desktop, false otherwise
        /// </summary>
        internal static bool IsLinuxDesktop
        {
#if ANDROID
            get => false;
#else
            get => IsLinux;
#endif
        }

        /// <summary>
        /// Returns true if running on macOS, false otherwise
        /// </summary>
        internal static bool IsMac
        {
#if NET45 || UWP
            get => false; // no easy way to detect Mac platform host at runtime under net471
#else
            get => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#endif
        }

        /// <summary>
        /// Returns true if running in 64bit process, false otherwise
        /// </summary>
        internal static bool IsX64BitProcess => IntPtr.Size == 8;

        /// Following code is from Code from DesktopBridgeHelpers

        const long APPMODEL_ERROR_NO_PACKAGE = 15700L;

        [DllImport(Constants.Kernel32, CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder packageFullName);

        static bool isUwp;
        static bool uwpChecked;

        internal static bool IsUWP
        {
            get
            {
                if (uwpChecked)
                    return isUwp;

                uwpChecked = true;

                if (IsWindows7OrLower)
                {
                    isUwp = false;
                }
                else
                {
                    var length = 0;
                    var sb = new StringBuilder(length);
                    isUwp = GetCurrentPackageFullName(ref length, sb) != APPMODEL_ERROR_NO_PACKAGE;
                }
                return isUwp;
            }
        }

        private static bool IsWindows7OrLower
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
