using System;
using LibVLCSharp.Platforms.Windows;
using LibVLCSharp.Shared;

namespace LibVLCSharp.Uno
{
    /// <summary>
    /// VideoView implementation for the UWP platform
    /// </summary>
    public class VideoView : VideoView<InitializedEventArgs>, IVideoView, IVideoControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoView"/> class.
        /// </summary>
        public VideoView()
        {
            // Re-key the default style to this type. VideoViewBase keys it to
            // LibVLCSharp.Platforms.Windows.VideoView and does not resolve from here; the SwapChainPanel
            // template for this type is in Themes/VideoView.UWP.xaml instead.
            DefaultStyleKey = typeof(VideoView);
        }

        /// <summary>
        /// Creates args for <see cref="VideoView{TInitializedEventArgs}.Initialized"/> event
        /// </summary>
        /// <returns>args for <see cref="VideoView{TInitializedEventArgs}.Initialized"/> event</returns>
        protected override InitializedEventArgs CreateInitializedEventArgs()
        {
            return new InitializedEventArgs(SwapChainOptions);
        }

        double IVideoControl.Width => ActualWidth;
        double IVideoControl.Height => ActualHeight;

        private EventHandler? _sizeChangedHandler;
        event EventHandler IVideoControl.SizeChanged
        {
            add
            {
                _sizeChangedHandler += value;
                SizeChanged += VideoView_SizeChanged;
            }

            remove
            {
                _sizeChangedHandler -= value;
                SizeChanged -= VideoView_SizeChanged;
            }
        }

        private void VideoView_SizeChanged(object? sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
        {
            _sizeChangedHandler?.Invoke(this, EventArgs.Empty);
        }
    }
}
