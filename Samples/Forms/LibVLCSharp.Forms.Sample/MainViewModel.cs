using LibVLCSharp.Shared;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LibVLCSharp.Forms.Sample
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            Task.Run(Initialize);
        }

        private LibVLC LibVLC { get; set; }

        private MediaPlayer _mediaPlayer;
        public MediaPlayer MediaPlayer
        {
            get => _mediaPlayer;
            private set => Set(nameof(MediaPlayer), ref _mediaPlayer, value);
        }

        private bool _isBuffering;
        public bool IsBuffering
        {
            get => _isBuffering;
            private set => Set(nameof(IsBuffering), ref _isBuffering, value);
        }

        private bool IsLoaded { get; set; }
        private bool IsVideoViewInitialized { get; set; }

        private void Set<T>(string propertyName, ref T field, T value)
        {
            if (field == null && value != null || field != null && !field.Equals(value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Initialize()
        {
            Core.Initialize();

            var options = new List<string>
            {
                "--verbose=2", "--force-equirectangular"
            };

            if(Device.RuntimePlatform == Device.Android)
            {
                options.Add("--vout=gles2");
            }

            LibVLC = new LibVLC(options.ToArray());
            LibVLC.Log += LibVLC_Log;
            var media = new Media(LibVLC,
                    "http://40.121.205.100:1935/live/video_small_optimized/playlist.m3u8",
                    FromType.FromLocation);

            MediaPlayer = new MediaPlayer(LibVLC)
            {
                Media = media
            };

            MediaPlayer.Buffering += MediaPlayer_Buffering;
        }

        private void MediaPlayer_Buffering(object sender, MediaPlayerBufferingEventArgs e) => IsBuffering = e.Cache != 100;
        private void LibVLC_Log(object sender, LogEventArgs e)
        {
            Debug.WriteLine(e.Message);
        }

        public void OnAppearing()
        {
            IsLoaded = true;
            Play();
        }

        public void OnVideoViewInitialized()
        {
            IsVideoViewInitialized = true;
            Play();
        }

        private void Play()
        {
            if (IsLoaded && IsVideoViewInitialized)
            {
                MediaPlayer.Play();
            }
        }
    }
}
