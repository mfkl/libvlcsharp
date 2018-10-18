using LibVLCSharp.Platforms.iOS;
using LibVLCSharp.Shared;
using ObjCRuntime;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UIKit;

namespace LibVLCSharp.iOS.Sample
{
    public class ViewController : UIViewController
    {
        VideoView _videoView;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _videoView = new VideoView();
            _videoView.LibVLC.SetLog(Logcb);
            View = _videoView;

            _videoView.MediaPlayer.Play(new Media(_videoView.LibVLC, "http://www.quirksmode.org/html5/videos/big_buck_bunny.mp4", Media.FromType.FromLocation));
        }

        [MonoPInvokeCallback(typeof(LogCallback))]
        private static void Logcb(IntPtr data, LogLevel logLevel, IntPtr logContext, string format, IntPtr args)
        {
            IntPtr str = IntPtr.Zero;

            vasprintf(ref str, Utf8StringMarshaler.GetInstance().MarshalManagedToNative(format), args);
            var message = Utf8StringMarshaler.GetInstance().MarshalNativeToManaged(str);
            Debug.WriteLine(message);
        }

        [DllImport("/usr/lib/libc.dylib")]
        static extern int vasprintf(ref IntPtr str, IntPtr format, IntPtr args);
    }
}