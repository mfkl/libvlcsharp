using LibVLCSharp.Shared;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace LibVLCSharp.WinForms
{
    /// <summary>
    /// WinForms VideoView control with a LibVLCSharp MediaPlayer
    /// </summary>
    public class VideoView : Control, ISupportInitialize, IVideoView, IDisposable
    {
        /// <summary>
        /// The VideoView constructor.
        /// </summary>
        public VideoView()
        {
            BackColor = System.Drawing.Color.Black;
        }

        MediaPlayer _mp;

        /// <summary>
        /// The MediaPlayer attached to this view (or null)
        /// </summary>
        public MediaPlayer MediaPlayer
        {
            get => _mp;
            set
            {
                if (ReferenceEquals(_mp, value))
                {
                    return;
                }

                Detach();
                _mp = value;
                Attach();
            }
        }

        /// <summary>
        /// This currently does not do anything
        /// </summary>
        void ISupportInitialize.BeginInit()
        {
        }

        /// <summary>
        /// This attaches the mediaplayer to the view (if any)
        /// </summary>
        void ISupportInitialize.EndInit()
        {
            if (IsInDesignMode)
                return;

            Attach();
        }

        bool IsInDesignMode
        {
            get
            {
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                    return true;

                Control ctrl = this;
                while (ctrl != null)
                {
                    if ((ctrl.Site != null) && ctrl.Site.DesignMode)
                        return true;
                    ctrl = ctrl.Parent;
                }
                return false;
            }
        }

        void Detach()
        {
            if (_mp == null)
                return;

            if(PlatformHelper.IsWindows)
            {
                _mp.Hwnd = IntPtr.Zero;
            }
            else if(PlatformHelper.IsLinux)
            {
                _mp.XWindow = 0;
            }
            else if(PlatformHelper.IsMac)
            {
                _mp.NsObject = IntPtr.Zero;
            }
        }

        void Attach()
        {
            if (_mp == null)
                return;

            if(PlatformHelper.IsWindows)
            {
                _mp.Hwnd = Handle;
            }
            else if(PlatformHelper.IsLinux)
            {
                _mp.XWindow = (uint)Handle;
            }
            else if(PlatformHelper.IsMac)
            {
                _mp.NsObject = Handle;
            }
        }

        bool disposedValue;
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Detach();
                }
                
                disposedValue = true;
            }
        }   
    }
}