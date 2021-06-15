using OpenTK.Windowing.Desktop;
using System;

namespace LibVLCSharp.CustomRendering.OpenGL
{
    class Program
    {
        static void Main(string[] args)
        {
            using Game game = new Game(new GameWindowSettings(), new NativeWindowSettings() { Title = "LibVLCSharp OpenGL sample" });
            game.Run();
        }
    }

    public class Game : GameWindow
    {
        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }
    }
}
