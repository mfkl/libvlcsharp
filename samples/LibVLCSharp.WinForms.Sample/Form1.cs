using System;
using System.Windows.Forms;
using LibVLCSharp.Shared;
using libVlcSharp.Samples;

namespace LibVLCSharp.WinForms.Sample
{
    public partial class Form1 : Form
    {
        public LibVLC _libVLC;
        public MediaPlayer _mp;

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
            var media = new Media(_libVLC, new Uri(Consts.SampleVideoUrl));
            _mp.Play(media);
            media.Dispose();
        }
    }
}
