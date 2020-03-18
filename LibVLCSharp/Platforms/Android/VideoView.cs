using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using LibVLCSharp.Shared;

using Org.Videolan.Libvlc;
using Orientation = Android.Content.Res.Orientation;

namespace LibVLCSharp.Platforms.Android
{
    /// <summary>
    ///
    /// </summary>
    public interface IOnNewVideoLayoutListener
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="vlcVout"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="visibleWidth"></param>
        /// <param name="visibleHeight"></param>
        /// <param name="sarNum"></param>
        /// <param name="sarDen"></param>
        void OnNewVideoLayout(IVLCVout vlcVout, int width, int height,
                              int visibleWidth, int visibleHeight, int sarNum, int sarDen);
    }

    /// <summary>
    ///
    /// </summary>
    public class VideoHelper : IOnNewVideoLayoutListener
    {
        private int mVideoWidth;
        private int mVideoHeight;
        private int mVideoVisibleWidth;
        private int mVideoVisibleHeight;
        private int mVideoSarNum;
        private int mVideoSarDen;
        private IVLCVout mVlcVout;

        private FrameLayout mVideoSurfaceFrame;
        private SurfaceView mVideoSurface = null;
        private SurfaceView mSubtitlesSurface = null;
        private TextureView mVideoTexture = null;

        /// <summary>
        ///
        /// </summary>
        /// <param name="surfaceFrame"></param>
        /// <param name="subtitles"></param>
        /// <param name="textureView"></param>
        public VideoHelper(VLCVideoLayout surfaceFrame, bool subtitles, bool textureView)
        {
            mVideoSurfaceFrame = surfaceFrame.FindViewById<FrameLayout>(Resource.Id.player_surface_frame);

            if (!textureView)
            {
                var stub = mVideoSurfaceFrame.FindViewById<ViewStub>(Resource.Id.surface_stub);
                mVideoSurface = stub != null ? (SurfaceView)stub.Inflate()
                    : mVideoSurfaceFrame.FindViewById<SurfaceView>(Resource.Id.surface_video);
                if (subtitles)
                {
                    stub = mVideoSurfaceFrame.FindViewById<ViewStub>(Resource.Id.subtitles_surface_stub);
                    mSubtitlesSurface = stub != null ? (SurfaceView)stub.Inflate()
                        : mVideoSurfaceFrame.FindViewById<SurfaceView>(Resource.Id.surface_subtitles);
                    mSubtitlesSurface.SetZOrderMediaOverlay(true);
                    mSubtitlesSurface.Holder.SetFormat(Format.Translucent);
                }
            }
            else
            {
                var stub = mVideoSurfaceFrame.FindViewById<ViewStub>(Resource.Id.texture_stub);
                mVideoTexture = stub != null ? (TextureView)stub.Inflate()
                    : mVideoSurfaceFrame.FindViewById<TextureView>(Resource.Id.texture_video);
                ;
            }

        }

//        void attachViews()
//        {
//            if (mVideoSurface == null && mVideoTexture == null)
//                return;
//            final IVLCVout vlcVout = mMediaPlayer.getVLCVout();
//            if (mVideoSurface != null)
//            {
//                vlcVout.setVideoView(mVideoSurface);
//                if (mSubtitlesSurface != null)
//                    vlcVout.setSubtitlesView(mSubtitlesSurface);
//            }
//            else if (mVideoTexture != null)
//                vlcVout.setVideoView(mVideoTexture);
//            else
//                return;
//            vlcVout.attachViews(this);

//            if (mOnLayoutChangeListener == null)
//            {
//                mOnLayoutChangeListener = new View.OnLayoutChangeListener()
//                {
//                private final Runnable runnable = new Runnable()
//        {
//            @Override
//                    public void run()
//            {
//                if (mVideoSurfaceFrame != null && mOnLayoutChangeListener != null)
//                    updateVideoSurfaces();
//            }
//        };
//        @Override
//                public void onLayoutChange(View v, int left, int top, int right,
//                                           int bottom, int oldLeft, int oldTop, int oldRight, int oldBottom)
//        {
//            if (left != oldLeft || top != oldTop || right != oldRight || bottom != oldBottom)
//            {
//                mHandler.removeCallbacks(runnable);
//                mHandler.post(runnable);
//            }
//        }
//    };
//}
//mVideoSurfaceFrame.addOnLayoutChangeListener(mOnLayoutChangeListener);
//        mMediaPlayer.setVideoTrackEnabled(true);
//    }

//    void detachViews()
//{
//    if (mOnLayoutChangeListener != null && mVideoSurfaceFrame != null)
//    {
//        mVideoSurfaceFrame.removeOnLayoutChangeListener(mOnLayoutChangeListener);
//        mOnLayoutChangeListener = null;
//    }
//    mMediaPlayer.setVideoTrackEnabled(false);
//    mMediaPlayer.getVLCVout().detachViews();
//}


/// <summary>
///
/// </summary>
/// <param name="vlcVout"></param>
/// <param name="width"></param>
/// <param name="height"></param>
/// <param name="visibleWidth"></param>
/// <param name="visibleHeight"></param>
/// <param name="sarNum"></param>
/// <param name="sarDen"></param>
public void OnNewVideoLayout(IVLCVout vlcVout, int width, int height, int visibleWidth, int visibleHeight, int sarNum, int sarDen)
        {
            mVideoWidth = width;
            mVideoHeight = height;
            mVideoVisibleWidth = visibleWidth;
            mVideoVisibleHeight = visibleHeight;
            mVideoSarNum = sarNum;
            mVideoSarDen = sarDen;
            mVlcVout = vlcVout;
            UpdateVideoSurfaces();
        }

        private void UpdateVideoSurfaces()
        {
            if (mVlcVout.AreViewsAttached())
                return;

            // get screen size
            var sw = mVideoSurfaceFrame.Width;
            var sh = mVideoSurfaceFrame.Height;

            // sanity check
            if (sw * sh == 0)
            {
                System.Diagnostics.Debug.WriteLine("error surface size 0");
                return;
            }

            mVlcVout.SetWindowSize(sw, sh);

            var lp = mVideoSurface.LayoutParameters;

            double dw = sw, dh = sh;

            var isPortrait = mVideoSurfaceFrame.Resources.Configuration.Orientation == Orientation.Portrait;

            if (sw > sh && isPortrait || sw < sh && !isPortrait)
            {
                dw = sh;
                dh = sw;
            }

            // compute the aspect ratio
            double ar, vw;
            if (mVideoSarDen == mVideoSarNum)
            {
                /* No indication about the density, assuming 1:1 */
                vw = mVideoVisibleWidth;
                ar = mVideoVisibleWidth / mVideoVisibleHeight;
            }
            else
            {
                /* Use the specified aspect ratio */
                vw = mVideoVisibleWidth * mVideoSarNum / mVideoSarDen;
                ar = vw / mVideoVisibleHeight;
            }

            // compute the display aspect ratio
            var dar = dw / dh;
            if (dar < ar)
                dh = dw / ar;
            else
                dw = dh * ar;

            // set display size
            lp.Width = (int)Math.Ceiling(dw * mVideoWidth / mVideoVisibleWidth);
            lp.Height = (int)Math.Ceiling(dh * mVideoHeight / mVideoVisibleHeight);
            mVideoSurface.LayoutParameters = lp;
            if (mSubtitlesSurface != null)
                mSubtitlesSurface.LayoutParameters = lp;
            mVideoSurface.Invalidate();
            if (mSubtitlesSurface != null)
                mSubtitlesSurface.Invalidate();
        }
    }

    /// <summary>
    /// VideoView implementation for the Android platform
    /// </summary>
    public class VideoView : SurfaceView, IVLCVoutCallback, IVideoView, AWindow.ISurfaceCallback
    {
        MediaPlayer _mediaPlayer;
        AWindow _awindow;
        LayoutChangeListener _layoutListener;

        #region ctors

        /// <summary>
        /// Standard Java object constructor
        /// </summary>
        /// <param name="javaReference">java reference</param>
        /// <param name="transfer">JNI transfer ownership</param>
        public VideoView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        /// <summary>
        /// Standard Java object constructor
        /// </summary>
        /// <param name="context">Android context</param>
        public VideoView(Context context) : base(context)
        {
        }

        /// <summary>
        /// Standard Java object constructor
        /// </summary>
        /// <param name="context">Android context</param>
        /// <param name="attrs">collection of attributes</param>
        public VideoView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        /// <summary>
        /// Standard Java object constructor
        /// </summary>
        /// <param name="context">Android context</param>
        /// <param name="attrs">collection of attributes</param>
        /// <param name="defStyleAttr">style definition attribute</param>
        public VideoView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        /// <summary>
        /// Standard Java object constructor
        /// </summary>
        /// <param name="context">Android context</param>
        /// <param name="attrs">collection of attributes</param>
        /// <param name="defStyleAttr">style definition attribute</param>
        /// <param name="defStyleRes">style resolution attribute</param>
        public VideoView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        #endregion

        /// <summary>
        /// The MediaPlayer object attached to this VideoView. Use this to manage playback and more
        /// </summary>
        public MediaPlayer MediaPlayer
        {
            get => _mediaPlayer;
            set
            {
                if (_mediaPlayer != value)
                {
                    Detach();
                    _mediaPlayer = value;

                    if (_mediaPlayer != null)
                    {
                        Attach();
                    }
                }
            }
        }

        void Attach()
        {
            if (_mediaPlayer == null)
                throw new NullReferenceException(nameof(_mediaPlayer));

            var vh = new VideoHelper(new VLCVideoLayout(Context), true, false);



            //_awindow = new AWindow(this);
            //_awindow.AddCallback(this);
            //_awindow.SetVideoView(this);
            //_awindow.AttachViews();

            _mediaPlayer.SetAndroidContext(_awindow.Handle);

            //_layoutListener = new LayoutChangeListener(_awindow);
            //AddOnLayoutChangeListener(_layoutListener);
        }

        void Detach()
        {
            _awindow?.RemoveCallback(this);
            _awindow?.DetachViews();

            if (_layoutListener != null)
                RemoveOnLayoutChangeListener(_layoutListener);

            _layoutListener?.Dispose();
            _layoutListener = null;

            _awindow?.Dispose();
            _awindow = null;
        }

        /// <summary>
        /// This is to workaround the first layout change not being raised when VideoView is behind a Xamarin.Forms custom renderer on Android.
        /// </summary>
        public void TriggerLayoutChangeListener() => _awindow?.SetWindowSize(Width, Height);

        /// <summary>
        /// Callback when surfaces are created
        /// </summary>
        /// <param name="vout">Video output</param>
        public virtual void OnSurfacesCreated(IVLCVout vout)
        {
        }

        /// <summary>
        /// Callback when surfaces are destroyed
        /// </summary>
        /// <param name="vout">Video output</param>
        public virtual void OnSurfacesDestroyed(IVLCVout vout)
        {
        }

        /// <summary>
        /// Detach the mediaplayer from the view and dispose the view
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Detach();
        }

        void AWindow.ISurfaceCallback.OnSurfacesCreated(AWindow aWindow)
        {
        }

        void AWindow.ISurfaceCallback.OnSurfacesDestroyed(AWindow aWindow)
        {
        }
    }
}
