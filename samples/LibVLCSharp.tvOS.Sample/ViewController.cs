﻿using LibVLCSharp.Platforms.tvOS;
using LibVLCSharp;

using UIKit;

namespace LibVLCSharp.tvOS.Sample
{
    public class ViewController : UIViewController
    {
        VideoView _videoView;
        LibVLC _libVLC;
        MediaPlayer _mediaPlayer;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);

            _videoView = new VideoView { MediaPlayer = _mediaPlayer };

            View = _videoView;

            _videoView.MediaPlayer.Play(new Media(_libVLC, "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4", FromType.FromLocation));
        }
    }
}
