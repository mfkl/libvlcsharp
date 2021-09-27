using LibVlcTestShell.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LibVlcTestShell.Views
{
    public partial class AboutTwoPage : ContentPage
    {
        public AboutTwoPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                await ((AboutViewModel)BindingContext).OnAppearing();
            }
            catch (Exception) { }
            //your code here;

        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            try
            {
                ((AboutViewModel)BindingContext).OnDisappearing();
            }
            catch (Exception) { }
            //your code here;

        }
    }
}