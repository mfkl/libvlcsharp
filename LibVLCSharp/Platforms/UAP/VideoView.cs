using System;
using LibVLCSharp.Shared;
//using LibVLCSharp_UWP;
using Windows.UI.Xaml.Controls;

namespace LibVLCSharp.Platforms.UAP
{
    public class VideoView : SwapChainPanel, IVideoView
    {
        MediaPlayer _mp;
                
        public MediaPlayer MediaPlayer
        {
            get => _mp;
            set
            {
                if (ReferenceEquals(_mp, value))
                {
                    return;
                }

                Detach();
                _mp = value;
                Attach();
            }
        }

        private void Attach()
        {
            //var x = new DirectXManager();

            throw new NotImplementedException();
        }

        private void Detach()
        {
            throw new NotImplementedException();
        }
    }
}
