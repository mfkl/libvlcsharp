using System.Collections.Generic;
using System.Reflection;
using LibVLCSharp.Forms.Platforms.WPF;
using LibVLCSharp.Forms.Shared;
using Xamarin.Forms.Platform.WPF;

namespace LibVLCSharp.Forms.MediaElement.Sample.WPF
{
    public partial class MainWindow : FormsApplicationPage
    {
        public MainWindow()
        {
            InitializeComponent();
            InitDependencies();
            Xamarin.Forms.Forms.Init();
            LoadApplication(new Forms.Sample.MediaPlayerElement.App());
        }

        void InitDependencies()
        {
            var init = new List<Assembly>
            {
                typeof(VideoView).Assembly,
                typeof(VideoViewRenderer).Assembly
            };
        }
    }
}
