﻿using System;

namespace LibVLCSharp
{
    internal static class Constants
    {
#if IOS
        internal const string LibraryName = "@rpath/DynamicMobileVLCKit.framework/DynamicMobileVLCKit";
#elif TVOS
        internal const string LibraryName = "@rpath/TVVLCKit.framework/TVVLCKit";
#elif UNITY_IOS
        internal const string LibraryName = "@rpath/vlc.framework/vlc";
#else
        internal const string LibraryName = "libvlc";
#endif

#if UNITY_IOS
        internal const string CoreLibraryName = "@rpath/vlccore.framework/vlccore";
#else
        internal const string CoreLibraryName = "libvlccore";
#endif

        /// <summary>
        /// The name of the folder that contains the per-architecture folders
        /// </summary>
        internal const string LibrariesRepositoryFolderName = "libvlc";

        internal const string Msvcrt = "msvcrt";
        internal const string Libc = "libc";
        internal const string LibSystem = "libSystem";
        internal const string Kernel32 = "kernel32";
        internal const string WindowsLibraryExtension = ".dll";
        internal const string MacLibraryExtension = ".dylib";
        internal const string Lib = "lib";
        internal const string LibVLC = "libvlc";
#if UNITY_IOS
        internal const string UnityPlugin = "@rpath/VLCUnityPlugin.framework/VLCUnityPlugin";
#else
        internal const string UnityPlugin = "VLCUnityPlugin";
#endif
    }

    internal static class ArchitectureNames
    {
        internal const string Win64 = "win-x64";
        internal const string Win86 = "win-x86";
        internal const string MacOS64 = "osx-x64";
    }

    [Flags]
    internal enum ErrorModes : uint
    {
        SYSTEM_DEFAULT = 0x0,
        SEM_FAILCRITICALERRORS = 0x0001,
        SEM_NOALIGNMENTFAULTEXCEPT = 0x0004,
        SEM_NOGPFAULTERRORBOX = 0x0002,
        SEM_NOOPENFILEERRORBOX = 0x8000
    }
}

