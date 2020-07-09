using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using LibVLCSharp.Shared;

namespace LibVLCSharp.NetCore.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Core.Initialize();

            using var libVLC = new LibVLC("-vv");
            libVLC.Log += (s, e) => System.Diagnostics.Debug.WriteLine(e.FormattedLog);
            var FileStream = new FileStream(path: @"C:\Users\Martin\Downloads\video.ded", mode: FileMode.Open, access: FileAccess.Read);
            RijndaelManaged AES = new RijndaelManaged();
            SHA256 SHA256 = SHA256.Create();

            // read key from TextBox4.Text
            AES.Key = SHA256.ComputeHash(Encoding.ASCII.GetBytes("U[#x5:jg0$e-^etBx#MjWH5Zu_ndd9"));
            AES.Mode = CipherMode.ECB;


            // cryptostream starts here
            var cryptoStream = new CryptoStream(FileStream, AES.CreateDecryptor(), CryptoStreamMode.Read);
            using var input = new StreamMediaInput(cryptoStream);
            using var media = new Media(libVLC, input);
            using var mp = new MediaPlayer(media);
            mp.Play();
            Console.ReadKey();
        }
    }
}
