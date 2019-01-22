using LibVLCSharp.Shared;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace LibVLCSharp.WinForms
{
    public class VideoView : Control, ISupportInitialize, IVideoView, IDisposable
    {
        public VideoView()
        {
            BackColor = System.Drawing.Color.Black;
        }

        MediaPlayer _mp;

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

        void Detach()
        {
            if (_mp == null)
                return;

            _mp.Hwnd = IntPtr.Zero;
        }

        void Attach()
        {
            if (_mp == null)
                return;

            _mp.Hwnd = Handle;
        }

        public void BeginInit()
        {
        }

        public void EndInit()
        {
            if (IsInDesignMode)
                return;

            if(MediaPlayer != null)
                MediaPlayer.Hwnd = Handle;
        }

        private bool IsInDesignMode
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

        #region IDisposable Support

        bool disposedValue;
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (MediaPlayer != null)
                    {
                        MediaPlayer.Hwnd = IntPtr.Zero;
                    }
                }
                
                disposedValue = true;
            }
        }

        #endregion
    }
}
