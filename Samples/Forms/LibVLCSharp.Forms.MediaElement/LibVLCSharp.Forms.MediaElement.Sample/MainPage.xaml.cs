using Xamarin.Forms;

namespace LibVLCSharp.Forms.Sample.MediaPlayerElement
{
    /// <summary>
    /// Represents the main page.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        private void ContentPage_Appearing(object sender, System.EventArgs e)
        {
            base.OnAppearing();
            var vm = (MainViewModel)BindingContext;
            vm.Init();
        }
    }
}
