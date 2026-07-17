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
            // The base VideoViewBase.OnApplyTemplate expects a "SwapChainPanel" template part. That
            // part's default template lives in the LibVLCSharp (WinUI) assembly's Themes/Generic.xaml,
            // keyed by DefaultStyleKey = LibVLCSharp.Platforms.Windows.VideoView. Because this Uno
            // subclass lives in a *different* assembly, WinUI 3 does not resolve that cross-assembly
            // default style (LibVLCSharp.WinUI does not use GenerateLibraryLayout, so it isn't
            // addressable via ms-appx:///LibVLCSharp/...). The result: no template is applied,
            // OnApplyTemplate never runs, the SwapChainPanel is never created, and no video renders.
            // Assign the (trivial) template directly in code so it no longer depends on style lookup.
            Template = (Microsoft.UI.Xaml.Controls.ControlTemplate)Microsoft.UI.Xaml.Markup.XamlReader.Load(
                @"<ControlTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                   xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                      <SwapChainPanel x:Name=""SwapChainPanel"" />
                  </ControlTemplate>");
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
