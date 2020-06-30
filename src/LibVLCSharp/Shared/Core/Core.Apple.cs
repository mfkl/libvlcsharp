﻿using System;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace LibVLCSharp.Shared
{
#if IOS || TVOS
    /// <summary>
    /// The Core class handles libvlc loading intricacies on various platforms as well as
    /// the libvlc/libvlcsharp version match check.
    /// </summary>
    public static partial class Core
    {
        /// <summary>
        /// Load the native libvlc library (if necessary, depending on platform)
        /// <para/> Ensure that you installed the VideoLAN.LibVLC.[YourPlatform] package in your target project
        /// <para/> This will throw a <see cref="VLCException"/> if the native libvlc libraries cannot be found or loaded.
        /// <para/> It may also throw a <see cref="VLCException"/> if the LibVLC and LibVLCSharp major versions do not match.
        /// See https://code.videolan.org/videolan/LibVLCSharp/-/blob/master/docs/versioning.md for more info about the versioning strategy.
        /// </summary>
        /// <param name="libvlcDirectoryPath">The path to the directory that contains libvlc and libvlccore
        /// No need to specify unless running netstandard 1.1, or using custom location for libvlc
        /// <para/> This parameter is NOT supported on Linux, use LD_LIBRARY_PATH instead.
        /// </param>
        public static void Initialize(string? libvlcDirectoryPath = null)
        {
            EnsureVersionsMatch();
        }
    }
#elif MAC
     /// <summary>
    /// The Core class handles libvlc loading intricacies on various platforms as well as
    /// the libvlc/libvlcsharp version match check.
    /// </summary>
    public static partial class Core
    {
        static IntPtr LibvlcHandle;
        static IntPtr LibvlccoreHandle;

        /// <summary>
        /// Load the native libvlc library (if necessary, depending on platform)
        /// <para/> Ensure that you installed the VideoLAN.LibVLC.[YourPlatform] package in your target project
        /// <para/> This will throw a <see cref="VLCException"/> if the native libvlc libraries cannot be found or loaded.
        /// <para/> It may also throw a <see cref="VLCException"/> if the LibVLC and LibVLCSharp major versions do not match.
        /// See https://code.videolan.org/videolan/LibVLCSharp/-/blob/master/docs/versioning.md for more info about the versioning strategy.
        /// </summary>
        /// <param name="libvlcDirectoryPath">The path to the directory that contains libvlc and libvlccore
        /// No need to specify unless running netstandard 1.1, or using custom location for libvlc
        /// <para/> This parameter is NOT supported on Linux, use LD_LIBRARY_PATH instead.
        /// </param>
        public static void Initialize(string? libvlcDirectoryPath = null)
        {
            InitializeMac(libvlcDirectoryPath);

            EnsureVersionsMatch();
        }

        private static void InitializeMac(string? libvlcDirectoryPath)
        {
            if (!string.IsNullOrEmpty(libvlcDirectoryPath))
            {
                bool loadResult;

                var libvlcPath = LibVLCPath(libvlcDirectoryPath!);
                loadResult = LoadNativeLibrary(libvlcPath, out LibvlcHandle);
                if (!loadResult)
                    Log($"Failed to load required native libraries at {libvlcPath}");
                return;
            }

            var pluginPath = Path.Combine(Path.GetDirectoryName(typeof(LibVLC).Assembly.Location), "plugins");
            Debug.WriteLine($"VLC_PLUGIN_PATH: {pluginPath}");
            Environment.SetEnvironmentVariable("VLC_PLUGIN_PATH", pluginPath);

            var paths = ComputeLibVLCSearchPaths();

            foreach (var (libvlccore, libvlc) in paths)
            {
                var libvlcCoreLoadResult = LoadNativeLibrary(libvlccore, out LibvlccoreHandle);
                var libvlcLoadResult = LoadNativeLibrary(libvlc, out LibvlcHandle);
                if (libvlcLoadResult && libvlcCoreLoadResult)
                    break;
            }

            if (!Loaded)
            {
                throw new VLCException("Failed to load required native libraries. " +
                    $"{Environment.NewLine}Have you installed the latest LibVLC package from nuget for your target platform?" +
                    $"{Environment.NewLine}Search paths include {string.Join("; ", paths.Select(p => $"{p.libvlc},{p.libvlccore}"))}");
            }
        }
    }
#endif
}
