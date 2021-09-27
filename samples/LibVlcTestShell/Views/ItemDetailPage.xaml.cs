using LibVlcTestShell.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace LibVlcTestShell.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}