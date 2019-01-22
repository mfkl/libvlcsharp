using LibVLCSharp.Shared;
using System.Windows.Forms;

namespace LibVLCSharp.WinForms.Sample
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        LibVLC _libVLC;
        VideoView _videoView;
        MediaPlayer _mp;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            if(!DesignMode)
                Core.Initialize();

            _videoView = new VideoView();
            ((System.ComponentModel.ISupportInitialize)(_videoView)).BeginInit();
            SuspendLayout();

            _libVLC = new LibVLC("-vv");
            _mp = new MediaPlayer(_libVLC);
            Load += Form1_Load;

            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Text = "LibVLCSharp.WinForms";
            _videoView.Dock = DockStyle.Fill;
            Controls.Add(_videoView);
            _videoView.MediaPlayer = _mp;

            ((System.ComponentModel.ISupportInitialize)(_videoView)).EndInit();
            ResumeLayout(false);
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            _mp.Play(new Media(_libVLC, "http://www.quirksmode.org/html5/videos/big_buck_bunny.mp4", Media.FromType.FromLocation));
        }

        #endregion
    }
}