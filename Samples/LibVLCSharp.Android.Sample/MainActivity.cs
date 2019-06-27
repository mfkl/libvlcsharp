using Android.App;
using Android.OS;
using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;

namespace LibVLCSharp.Android.Sample
{
    [Activity(Label = "LibVLCSharp.Android.Sample", MainLauncher = true)]
    public class MainActivity : Activity
    {
        LibVLC _libVLC;
        List<MediaDiscoverer> _mediaDiscoverers = new List<MediaDiscoverer>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);            
        }

        protected override void OnResume()
        {
            base.OnResume();

            Core.Initialize();

            _libVLC = new LibVLC("--verbose=2");

            foreach (var md in _libVLC.MediaDiscoverers(MediaDiscovererCategory.Lan))
            {
                var discoverer = new MediaDiscoverer(_libVLC, md.Name);
                discoverer.MediaList.ItemAdded += MediaList_ItemAdded;
                _mediaDiscoverers.Add(discoverer);
            }

            foreach (var md in _mediaDiscoverers)
                md.Start();
        }

        private void MediaList_ItemAdded(object sender, MediaListItemAddedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("FOUND --------------------------> " + e.Media);
        }
    }
}