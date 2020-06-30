#if NETFRAMEWORK || NETSTANDARD
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace LibVLCSharp.Shared
{
    /// <summary>
    /// The Core class handles libvlc loading intricacies on various platforms as well as
    /// the libvlc/libvlcsharp version match check.
    /// </summary>
    public static partial class Core
    {
        partial struct Native
        {
            [DllImport(Constants.Kernel32, SetLastError = true)]
            internal static extern IntPtr LoadLibrary(string dllToLoad);

            [DllImport(Constants.LibSystem, EntryPoint = "dlopen")]
            internal static extern IntPtr Dlopen(string libraryPath, int mode = 1);

            /// <summary>
            /// Initializes the X threading system
            /// </summary>
            /// <remarks>Linux X11 only</remarks>
            /// <returns>non-zero on success, zero on failure</returns>
            [DllImport(Constants.LibX11, CallingConvention = CallingConvention.Cdecl)]
            internal static extern int XInitThreads();

            [DllImport(Constants.Kernel32, SetLastError = true)]
            internal static extern ErrorModes SetErrorMode(ErrorModes uMode);
        }

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
            DisableMessageErrorBox();
            InitializeDesktop(libvlcDirectoryPath);
#if !NETSTANDARD1_1
            EnsureVersionsMatch();
#endif
        }

        /// <summary>
        /// Disable error dialogs in case of dll loading failures on older Windows versions.
        /// <para/>
        /// This is mostly to fix Windows XP support (https://code.videolan.org/videolan/LibVLCSharp/issues/173),
        /// though it may happen under other conditions (broken plugins/wrong ABI).
        /// <para/>
        /// As libvlc may load additional plugins later in the lifecycle of the application,
        /// we should not unset this on exiting <see cref="Initialize(string)"/>
        /// </summary>
        static void DisableMessageErrorBox()
        {
            if (!PlatformHelper.IsWindows)
                return;

            var oldMode = Native.SetErrorMode(ErrorModes.SYSTEM_DEFAULT);
            Native.SetErrorMode(oldMode | ErrorModes.SEM_FAILCRITICALERRORS | ErrorModes.SEM_NOOPENFILEERRORBOX);
        }

        static void InitializeDesktop(string? libvlcDirectoryPath = null)
        {
            if(PlatformHelper.IsLinux)
            {
                if (!string.IsNullOrEmpty(libvlcDirectoryPath))
                {
                    throw new InvalidOperationException($"Using {nameof(libvlcDirectoryPath)} is not supported on the Linux platform. " +
                        $"The recommended way is to have the libvlc librairies in /usr/lib. Use LD_LIBRARY_PATH if you need more customization");
                }
                // Initializes X threads before calling VLC. This is required for vlc plugins like the VDPAU hardware acceleration plugin.
                if (Native.XInitThreads() == 0)
                {
#if !NETSTANDARD1_1
                    Trace.WriteLine("XInitThreads failed");
#endif
                }
                return;
            }

            // full path to directory location of libvlc and libvlccore has been provided
            if (!string.IsNullOrEmpty(libvlcDirectoryPath))
            {
                bool loadResult;
                if(PlatformHelper.IsWindows)
                {
                    var libvlccorePath = LibVLCCorePath(libvlcDirectoryPath!);
                    loadResult = LoadNativeLibrary(libvlccorePath, out LibvlccoreHandle);
                    if(!loadResult)
                    {
                        Log($"Failed to load required native libraries at {libvlccorePath}");
                        return;
                    }
                }

                var libvlcPath = LibVLCPath(libvlcDirectoryPath!);
                loadResult = LoadNativeLibrary(libvlcPath, out LibvlcHandle);
                if(!loadResult)
                    Log($"Failed to load required native libraries at {libvlcPath}");
                return;
            }

#if !NETSTANDARD1_1
            var paths = ComputeLibVLCSearchPaths();

            foreach(var path in paths)
            {
                if (PlatformHelper.IsWindows)
                {
                    LoadNativeLibrary(path.libvlccore, out LibvlccoreHandle);
                }
                var loadResult = LoadNativeLibrary(path.libvlc, out LibvlcHandle);
                if (loadResult) break;
            }

            if (!Loaded)
            {
                throw new VLCException("Failed to load required native libraries. " +
                    $"{Environment.NewLine}Have you installed the latest LibVLC package from nuget for your target platform?" +
                    $"{Environment.NewLine}Search paths include {string.Join("; ", paths.Select(p => $"{p.libvlc},{p.libvlccore}"))}");
            }
#endif
        }

#if !NETSTANDARD1_1
        static List<(string libvlccore, string libvlc)> ComputeLibVLCSearchPaths()
        {
            var paths = new List<(string, string)>();
            string arch;

            if(PlatformHelper.IsMac)
            {
                arch = ArchitectureNames.MacOS64;
            }
            else
            {
                arch = PlatformHelper.IsX64BitProcess ? ArchitectureNames.Win64 : ArchitectureNames.Win86;
            }

            var libvlcDirPath1 = Path.Combine(Path.GetDirectoryName(typeof(LibVLC).Assembly.Location),
                Constants.LibrariesRepositoryFolderName, arch);

            var libvlccorePath1 = string.Empty;
            if (PlatformHelper.IsWindows)
            {
                libvlccorePath1 = LibVLCCorePath(libvlcDirPath1);
            }
            var libvlcPath1 = LibVLCPath(libvlcDirPath1);
            paths.Add((libvlccorePath1, libvlcPath1));

            var assemblyLocation = Assembly.GetEntryAssembly()?.Location ?? Assembly.GetExecutingAssembly()?.Location;

            var libvlcDirPath2 = Path.Combine(Path.GetDirectoryName(assemblyLocation),
                Constants.LibrariesRepositoryFolderName, arch);

            var libvlccorePath2 = string.Empty;
            if(PlatformHelper.IsWindows)
            {
                libvlccorePath2 = LibVLCCorePath(libvlcDirPath2);
            }

            var libvlcPath2 = LibVLCPath(libvlcDirPath2);
            paths.Add((libvlccorePath2, libvlcPath2));

            var libvlcPath3 = LibVLCPath(Path.GetDirectoryName(typeof(LibVLC).Assembly.Location));

            paths.Add((string.Empty, libvlcPath3));
            return paths;
        }
#endif
        static string LibVLCCorePath(string dir) => Path.Combine(dir, $"{Constants.CoreLibraryName}{LibraryExtension}");

        static string LibVLCPath(string dir) => Path.Combine(dir, $"{Constants.LibraryName}{LibraryExtension}");

        static string LibraryExtension => PlatformHelper.IsWindows ? Constants.WindowsLibraryExtension : Constants.MacLibraryExtension;

        static bool Loaded => LibvlcHandle != IntPtr.Zero;

        static void Log(string message)
        {
#if !NETSTANDARD1_1
            Trace.WriteLine(message);
#else
            Debug.WriteLine(message);
#endif
        }

        static bool LoadNativeLibrary(string nativeLibraryPath, out IntPtr handle)
        {
            handle = IntPtr.Zero;
            Log($"Loading {nativeLibraryPath}");

#if !NETSTANDARD1_1
            if (!File.Exists(nativeLibraryPath))
            {
                Log($"Cannot find {nativeLibraryPath}");
                return false;
            }
#endif
            if(PlatformHelper.IsMac)
            {
                handle = Native.Dlopen(nativeLibraryPath);
            }
            else
            {
                handle = Native.LoadLibrary(nativeLibraryPath);
            }

            return handle != IntPtr.Zero;
        }
    }
}
#endif // NETFRAMEWORK || NETSTANDARD
