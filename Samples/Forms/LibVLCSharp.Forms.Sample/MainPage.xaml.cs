using LibVLCSharp.Shared;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LibVLCSharp.Forms.Sample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                var options = VideoView.SwapChainOptions;
                ((MainViewModel)BindingContext).OnAppearing(Device.RuntimePlatform == Device.UWP ? options : null);
            });
           
        }

        private void VideoView_MediaPlayerChanged(object sender, MediaPlayerChangedEventArgs e)
        {
            ((MainViewModel)BindingContext).OnVideoViewInitialized();
        }
    }
}