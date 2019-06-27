using System;
using System.ComponentModel;
using System.Windows.Input;
using LibVLCSharp.Shared;
using Xamarin.Forms;

namespace LibVLCSharp.Forms.Sample.MediaPlayerElement
{
    /// <summary>
    /// Represents the main viewmodel.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            
        }

        private LibVLC _libVLC;
        /// <summary>
        /// Gets the <see cref="LibVLCSharp.Shared.LibVLC"/> instance.
        /// </summary>
        public LibVLC LibVLC
        {
            get => _libVLC;
            private set => Set(nameof(LibVLC), ref _libVLC, value);
        }

        private MediaPlayer _mediaPlayer;
        /// <summary>
        /// Gets the <see cref="LibVLCSharp.Shared.MediaPlayer"/> instance.
        /// </summary>
        public MediaPlayer MediaPlayer
        {
            get => _mediaPlayer;
            private set => Set(nameof(MediaPlayer), ref _mediaPlayer, value);
        }

        public void Init()
        {
            Core.Initialize();

            LibVLC = new LibVLC();

            var media = new Media(LibVLC,
                "http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_surround-fix.avi",
                FromType.FromLocation);

            MediaPlayer = new MediaPlayer(media) { EnableHardwareDecoding = true };
            MediaPlayer.Play();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(string propertyName, ref T field, T value)
        {
            if (field == null && value != null || field != null && !field.Equals(value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
