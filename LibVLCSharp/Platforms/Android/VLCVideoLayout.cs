using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
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

        void SetupLayout(Context context) => Inflate(context, Resource.Layout.vlc_video_layout, this);

        /// <summary>
        ///
        /// </summary>
        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            SetBackgroundColor(Color.Black);
            var lp = LayoutParameters;
            lp.Height = ViewGroup.LayoutParams.MatchParent;
            lp.Width = ViewGroup.LayoutParams.MatchParent;
            LayoutParameters = lp;
        }
    }
}
