﻿using System;
using System.Runtime.InteropServices;

namespace LibVLCSharp
{
    /// <summary>
    /// Small helper for determining the current platform
    /// </summary>
    public class PlatformHelper
    {
        /// <summary>
        /// Returns true if running on Windows, false otherwise
        /// </summary>
        public static bool IsWindows
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
        public static bool IsLinux
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
        public static bool IsLinuxDesktop
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
        public static bool IsMac
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
        public static bool IsX64BitProcess => IntPtr.Size == 8;

#if !NET45
        /// <summary>
        /// Returns true if running in ARM64 process, false otherwise
        /// </summary>
        public static bool IsArm64BitProcess => RuntimeInformation.ProcessArchitecture == Architecture.Arm64;
#endif
    }
}
