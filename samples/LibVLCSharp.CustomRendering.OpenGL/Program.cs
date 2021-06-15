using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL;
using System;
using System.Threading.Tasks;

namespace LibVLCSharp.CustomRendering.OpenGL
{
    unsafe class Program : GameWindow
    {
        static Program program;
        static LibVLC libvlc;
        static MediaPlayer mp;
        static uint width, height;
        static uint[] tex = new uint[3];
        static uint[] fbo = new uint[3];

        public Program(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        [STAThread]
        static void Main()
        {
            program = new Program(new GameWindowSettings { IsMultiThreaded = true }, new NativeWindowSettings { Title = "LibVLCSharp OpenGL sample" });
            program.Run();
        }

        /// <summary>
        /// https://github.com/opentk/opentk-examples/blob/master/src/BasicTriangle/Program.cs
        /// https://github.com/videolan/vlc/blob/master/doc/libvlc/sdl_opengl_player.cpp
        /// </summary>
        protected override void OnLoad()
        {
            Core.Initialize();

            libvlc = new LibVLC(enableDebugLogs: true);
            mp = new MediaPlayer(libvlc)
            {
                Media = new Media(libvlc, new Uri("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4"))
            };

            mp.SetOutputCallbacks(VideoEngine.OpenGL, Setup, Cleanup, null, UpdateOutput, Swap, MakeCurrent, GetProcAddress, null, null);

            mp.Play();

            base.OnLoad();
        }

        private static bool Setup(ref IntPtr opaque, SetupDeviceConfig* config, ref SetupDeviceInfo setup)
        {
            if (!program.IsMultiThreaded) return false;

            width = 0;
            height = 0;

            return true;
        }
        private static void Cleanup(IntPtr opaque)
        {
            if (width == 0 && height == 0)
                return;

            GL.DeleteTextures(3, tex);
            GL.DeleteFramebuffers(3, fbo);
        }
        private static bool UpdateOutput(IntPtr opaque, RenderConfig* config, ref OutputConfig output)
        {
            if (config->Width != width || config->Height != height)
                Cleanup(opaque);

            return true;
        }

        private static IntPtr GetProcAddress(IntPtr opaque, IntPtr functionName)
        {
            throw new NotImplementedException();
        }

        private static bool MakeCurrent(IntPtr opaque, bool enter)
        {
            throw new NotImplementedException();
        }

        private static void Swap(IntPtr opaque)
        {
            throw new NotImplementedException();
        }


        private static void OutputSetResize(IntPtr opaque, MediaPlayer.ReportSizeChange report_size_change, IntPtr report_opaque)
        {
            throw new NotImplementedException();
        }
    }
}
