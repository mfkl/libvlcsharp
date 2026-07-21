using System;
using Avalonia.Controls;
using LibVLCSharp.Shared;
using libVlcSharp.Samples;

namespace LibVLCSharp.Avalonia.Sample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IDisposable
    {
        private readonly LibVLC _libVlc = new LibVLC();
        
        public MediaPlayer MediaPlayer { get; }
        
        public MainWindowViewModel()
        {
            MediaPlayer = new MediaPlayer(_libVlc);
        }

        public void Play()
        {
            if (Design.IsDesignMode)
            {
                return;
            }
            
            using var media = new Media(_libVlc, new Uri(Consts.SampleVideoUrl));
            MediaPlayer.Play(media);
        }
        
        public void Stop()
        {            
            MediaPlayer.Stop();
        }
   
        public void Dispose()
        {
            MediaPlayer?.Dispose();
            _libVlc?.Dispose();
        }
    }
}
