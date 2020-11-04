using System;
using LibVLCSharp.Shared;
using Avalonia.Controls;
using Avalonia.Platform;
using System.Runtime.InteropServices;

namespace LibVLCSharp.Avalonia
{
    /// <summary>
    ///
    /// </summary>
    public class VideoView : NativeControlHost, IVideoView
    {
        private MediaPlayer? _mediaPlayer;

        /// <summary>
        /// The MediaPlayer property for that GTK VideoView
        /// </summary>
        public MediaPlayer? MediaPlayer
        {
            get
            {
                return _mediaPlayer;
            }
            set
            {
                if (ReferenceEquals(_mediaPlayer, value))
                {
                    return;
                }

                Detach();
                _mediaPlayer = value;
                Attach();
            }
        }

        private void Attach()
        {
            throw new NotImplementedException();
        }

        private void Detach()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return CreateLinux(parent);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return CreateWin32(parent);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return CreateOSX(parent);
            return base.CreateNativeControlCore(parent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="control"></param>
        protected override void DestroyNativeControlCore(IPlatformHandle control)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                DestroyLinux(control);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                DestroyWin32(control);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                DestroyOSX(control);
            else
                base.DestroyNativeControlCore(control);
        }
    }
}
