using LibVLCSharp.Shared;
using Xamarin.Forms;

namespace LibVLCSharp.Forms.Sample
{
    public partial class MainPage : ContentPage
    {
        private const float DEFAULT_FOV = 80f;
        private const float DIVIDER = 10f;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((MainViewModel)BindingContext).OnAppearing();
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var yaw = (float)(DEFAULT_FOV * -e.TotalX / videoView.Width) / DIVIDER;
            var pitch = (float)(DEFAULT_FOV * -e.TotalY / videoView.Height) / DIVIDER;

            ((MainViewModel)BindingContext).MediaPlayer.UpdateViewpoint(yaw, pitch, 0, 0, false);
        }

        void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Scale == 1)
                return;

            ((MainViewModel)BindingContext).MediaPlayer.UpdateViewpoint(0, 0, 0, e.Scale < 1 ? 1f : -1f, false);
        }

        private void VideoView_MediaPlayerChanged(object sender, MediaPlayerChangedEventArgs e)
        {
            ((MainViewModel)BindingContext).OnVideoViewInitialized();
        }
    }
}
