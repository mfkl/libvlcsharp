using System;
using LibVLCSharp;

namespace LibVLCSharp.NetCore.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            using var libVLC = new LibVLC(enableDebugLogs: true);
            using var media = new Media(new Uri("rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mp4"));
            using var mp1 = new MediaPlayer(libVLC, media);
            using var mp2 = new MediaPlayer(libVLC, media);
            using var mp3 = new MediaPlayer(libVLC, media);
            using var mp4 = new MediaPlayer(libVLC, media);
            mp1.Play();
            mp2.Play();
            mp3.Play();
            mp4.Play();
            Console.ReadKey();
        }
    }
}
