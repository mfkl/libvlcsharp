using LibVLCSharp.Shared;
using System;
using libVlcSharp.Samples;

namespace LibVLCSharp.Windows.Net40.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            using var libVLC = new LibVLC(enableDebugLogs: true);
            using var media = new Media(libVLC, new Uri(Consts.SampleVideoUrl));
            using var mp = new MediaPlayer(media);
            mp.Play();

            Console.ReadKey();
        }
    }
}
