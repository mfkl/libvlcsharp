using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using LibVLCSharp.Shared.Helpers;

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
            [DllImport(Constants.LibraryName, CallingConvention = CallingConvention.Cdecl,
                EntryPoint = "libvlc_get_version")]
            internal static extern IntPtr LibVLCVersion();
        }

#if !UWP10_0 && !NETSTANDARD1_1
        /// <summary>
        /// Checks whether the major version of LibVLC and LibVLCSharp match <para/>
        /// Throws an NotSupportedException if the major versions mismatch
        /// </summary>
        static void EnsureVersionsMatch()
        {
            var libvlcMajorVersion = int.Parse(Native.LibVLCVersion().FromUtf8()?.Split('.').FirstOrDefault() ?? "0");
            var libvlcsharpMajorVersion = Assembly.GetExecutingAssembly().GetName().Version.Major;
            if (libvlcMajorVersion != libvlcsharpMajorVersion)
                throw new VLCException($"Version mismatch between LibVLC {libvlcMajorVersion} and LibVLCSharp {libvlcsharpMajorVersion}. " +
                    $"They must share the same major version number");
        }
#endif
    }
}
