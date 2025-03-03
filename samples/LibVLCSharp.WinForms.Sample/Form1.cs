using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using LibVLCSharp.Shared;

namespace LibVLCSharp.WinForms.Sample
{
    public partial class Form1 : Form
    {
        public LibVLC _libVLC;
        public MediaPlayer _mp;
        // Windows API Constants
        private const int WS_EX_LAYERED = 0x00080000;
        private const int GWL_EXSTYLE = -20;
        private const int LWA_ALPHA = 0x2;

        // Windows API Imports
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetLayeredWindowAttributes(IntPtr hWnd, byte crKey, byte bAlpha, int dwFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetParent(IntPtr hWnd);

        public static void SetWindowOpacity(IntPtr hWnd, byte opacity)
        {
            int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            SetWindowLong(hWnd, GWL_EXSTYLE, exStyle | WS_EX_LAYERED);

            // Set window opacity (0 = fully transparent, 255 = fully opaque)
            SetLayeredWindowAttributes(hWnd, 0, opacity, LWA_ALPHA);
        }

        public Form1()
        {
            if (!DesignMode)
            {
                Core.Initialize();
            }

            InitializeComponent();
            _libVLC = new LibVLC();
            _mp = new MediaPlayer(_libVLC);
            videoView1.MediaPlayer = _mp;
            Load += Form1_Load;
            FormClosed += Form1_FormClosed;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _mp.Stop();
            _mp.Dispose();
            _libVLC.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var media = new Media(_libVLC, new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"));
            var parentHwnd = GetParent(videoView1.Handle);
            int exStyle = GetWindowLong(parentHwnd, GWL_EXSTYLE);
            SetWindowLong(parentHwnd, GWL_EXSTYLE, exStyle | WS_EX_LAYERED);

            SetWindowOpacity(parentHwnd, 128);
            _mp.Play(media);
            media.Dispose();
        }
    }
}
