using Uno.UI;

namespace LibVLCSharp.Platforms.Uno
{
    /// <summary>
    /// Video view
    /// </summary>
    public partial class VideoView
    {
        private Android.VideoView? UnderlyingVideoView
        {
            get;
            set;
        }

        private Android.VideoView CreateVideoView()
        {
            return new Android.VideoView(ContextHelper.Current);
        }
    }
}
