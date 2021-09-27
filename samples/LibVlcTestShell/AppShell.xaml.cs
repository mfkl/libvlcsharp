using LibVLCSharp.Shared;
using LibVlcTestShell.ViewModels;
using LibVlcTestShell.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LibVlcTestShell
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Core.Initialize();
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
