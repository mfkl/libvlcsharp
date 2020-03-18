using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace LibVLCSharp.Platforms.Android
{
    /// <summary>
    ///
    /// </summary>
    public class VLCVideoLayout : FrameLayout
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        public VLCVideoLayout(Context context) : base(context)
            => SetupLayout(context);

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attrs"></param>
        public VLCVideoLayout(Context context, IAttributeSet attrs) : base(context, attrs)
            => SetupLayout(context);

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attrs"></param>
        /// <param name="defStyleAttr"></param>
        public VLCVideoLayout(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
                => SetupLayout(context);

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attrs"></param>
        /// <param name="defStyleAttr"></param>
        /// <param name="defStyleRes"></param>
        public VLCVideoLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
                => SetupLayout(context);

        void SetupLayout(Context context)
        {
            var r = Resource.Layout.vlc_video_layout;
        }
    }
}
