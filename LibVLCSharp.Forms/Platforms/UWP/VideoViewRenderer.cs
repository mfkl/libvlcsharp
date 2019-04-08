using LibVLCSharp.Shared;
using LibVLCSharp.Forms.Shared;
using LibVLCSharp.Forms.Platforms.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(LibVLCSharp.Forms.Shared.VideoView), typeof(VideoViewRenderer))]
namespace LibVLCSharp.Forms.Platforms.UWP
{
    public class VideoViewRenderer : ViewRenderer<LibVLCSharp.Forms.Shared.VideoView, LibVLCSharp.Platforms.UWP.VideoView>
    {
        LibVLCSharp.Platforms.UWP.VideoView _videoView;

        protected override void OnElementChanged(ElementChangedEventArgs<VideoView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                _videoView = new LibVLCSharp.Platforms.UWP.VideoView();
                _videoView.Loaded += OnVideoViewLoaded;

                SetNativeControl(_videoView);
            }

            if (e.OldElement != null)
            {
                e.OldElement.MediaPlayerChanging -= OnMediaPlayerChanging;
            }

            if (e.NewElement != null)
            {
                e.NewElement.MediaPlayerChanging += OnMediaPlayerChanging;
                if (Control.MediaPlayer != e.NewElement.MediaPlayer)
                {
                    OnMediaPlayerChanging(this, new MediaPlayerChangingEventArgs(Control.MediaPlayer, e.NewElement.MediaPlayer));
                }
            }
        }

        private void OnVideoViewLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Element.SwapChainOptions = _videoView.SwapChainOptions;
        }

        private void OnMediaPlayerChanging(object sender, MediaPlayerChangingEventArgs e)
        {
            Control.MediaPlayer = e.NewMediaPlayer;
        }
    }
}