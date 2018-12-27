using Android.Views;
using Org.Videolan.Libvlc;

namespace LibVLCSharp.Platforms.Android
{
    /// <summary>
    /// LayoutChangeListener for AWindow
    /// </summary>
    public class LayoutChangeListener : Java.Lang.Object, View.IOnLayoutChangeListener
    {
        readonly AWindow _aWindow;

        /// <summary>
        /// LayoutChangeListener constructor
        /// </summary>
        /// <param name="awindow">The AWindow for this listener</param>
        public LayoutChangeListener(AWindow awindow)
        {
            _aWindow = awindow;
        }

        /// <summary>
        /// Called when the layout changes
        /// </summary>
        /// <param name="v"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="oldLeft"></param>
        /// <param name="oldTop"></param>
        /// <param name="oldRight"></param>
        /// <param name="oldBottom"></param>
        public void OnLayoutChange(View v, int left, int top, int right, int bottom, int oldLeft, int oldTop, int oldRight,
            int oldBottom)
        {
            if (left != oldLeft || top != oldTop || right != oldRight || bottom != oldBottom)
            {
                _aWindow.SetWindowSize(right - left, bottom - top);
            }

        }
    }
}