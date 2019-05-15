using LibVLCSharp.Platforms.iOS;
using LibVLCSharp.Shared;
using System.Diagnostics;
using UIKit;

namespace LibVLCSharp.iOS.Sample
{
    public class ViewController : UIViewController
    {
        VideoView _videoView;
        LibVLC _libVLC;
        Shared.MediaPlayer _mediaPlayer;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _libVLC = new LibVLC();
            _libVLC.Log += _libVLC_Log;
            _mediaPlayer = new Shared.MediaPlayer(_libVLC);

            _videoView = new VideoView { MediaPlayer = _mediaPlayer };

            View = _videoView;

            _videoView.MediaPlayer.Play(new Media(_libVLC, "http://www.quirksmode.org/html5/videos/big_buck_bunny.mp4", FromType.FromLocation));
        }

        private void _libVLC_Log(object sender, LogEventArgs e)
        {
            Debug.WriteLine($"{e.Level}: {e.Module} -> {e.Message}");
        }
    }
}