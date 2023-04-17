using System;
using System.Runtime.InteropServices;
using LibVLCSharp;
using Windows.Win32.System.Threading;
using Windows.Win32.Graphics.Dxgi;
using Windows.Win32.Graphics.Dxgi.Common;
using Windows.Win32.Graphics.Direct3D11;
using Windows.Win32.Graphics.Direct3D;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Direct3D10;
using Windows.System.Profile;

#if WINUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel;
#endif

namespace LibVLCSharp.Platforms.Windows
{
    /// <summary>
    /// VideoView class for the Windows platform
    /// </summary>
    [TemplatePart(Name = PartSwapChainPanelName, Type = typeof(SwapChainPanel))]
    public unsafe class VideoView : Control, IVideoView
    {
        private const string PartSwapChainPanelName = "SwapChainPanel";

        SwapChainPanel? _panel;
        //SharpDX.Direct3D11.Device? _d3D11Device;
        //SharpDX.DXGI.Device3? _device3;
        //SwapChain2? _swapChain2;
        //SwapChain1? _swapChain;
        //DeviceContext? _deviceContext;
        const string Mobile = "Windows.Mobile";
        bool _loaded;

        //IDXGISwapChain* _swapchain;
        //ID3D11RenderTargetView* _swapchainRenderTarget;

        //ID3D11Device* _d3dDevice;
        //ID3D11DeviceContext* _d3dctx;

        //int WIDTH = 1500;
        //int HEIGHT = 900;

        //ID3D11Device* _d3deviceVLC;
        //ID3D11DeviceContext* _d3dctxVLC;

        //ID3D11Texture2D* _textureVLC;
        //ID3D11RenderTargetView* _textureRenderTarget;
        //HANDLE _sharedHandle;
        //ID3D11Texture2D* _texture;
        //ID3D11ShaderResourceView* _textureShaderInput;

        /////* our vertex/pixel shader */
        //ID3D11VertexShader* pVS;
        //ID3D11PixelShader* pPS;
        //ID3D11InputLayout* pShadersInputLayout;

        //ID3D11Buffer* pVertexBuffer;
        //int vertexBufferStride;

        //uint quadIndexCount;
        //ID3D11Buffer* pIndexBuffer;

        //RTL_CRITICAL_SECTION sizeLock = new();

        //readonly float BORDER_LEFT = -0.95f;
        //readonly float BORDER_RIGHT = 0.85f;
        //readonly float BORDER_TOP = 0.95f;
        //readonly float BORDER_BOTTOM = -0.90f;

        //ID3D11SamplerState* samplerState;

        //MediaPlayer.ReportSizeChange? reportSize;
        //IntPtr reportOpaque;

        //uint width, height;

        /// <summary>
        /// The constructor
        /// </summary>
        public VideoView()
        {
            DefaultStyleKey = typeof(VideoView);

            Unloaded += (s, e) => DestroySwapChain();
#if !WINUI
            if (!DesignMode.DesignModeEnabled)
            {
                Application.Current.Suspending += (s, e) => { Trim(); };
            }
#endif
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. 
        /// In simplest terms, this means the method is called just before a UI element displays in your app.
        /// Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _panel = (SwapChainPanel)GetTemplateChild(PartSwapChainPanelName);

#if !WINUI
            if (DesignMode.DesignModeEnabled)
                return;
#endif
            DestroySwapChain();

            _panel.SizeChanged += (s, eventArgs) =>
            {
                if (_loaded)
                {
                    UpdateSize();
                }
                else
                {
                    CreateSwapChain();
                }
            };

            _panel.CompositionScaleChanged += (s, eventArgs) =>
            {
                if (_loaded)
                {
                    UpdateScale();
                }
            };

        }

        /// <summary>
        /// Initializes the SwapChain for use with LibVLC
        /// </summary>
        void CreateSwapChain()
        {
            // Do not create the swapchain when the VideoView is collapsed.
            if (_panel == null || _panel.ActualHeight == 0)
                return;

            var factory = new IDXGIFactory2();
            //factory.
            //factory.CreateSwapChainForComposition()

            factory.Release();
            //Windows.Win32.Graphics.Dxgi.Factory2? dxgiFactory = null;
            //try
            // {
            //                var deviceCreationFlags =
            //                    DeviceCreationFlags.BgraSupport | DeviceCreationFlags.VideoSupport;

            //#if DEBUG
            //                if (AnalyticsInfo.VersionInfo.DeviceFamily != Mobile)
            //                    deviceCreationFlags |= DeviceCreationFlags.Debug;

            //                try
            //                {
            //                    dxgiFactory = new SharpDX.DXGI.Factory2(true);
            //                }
            //                catch (SharpDXException)
            //                {
            //                    dxgiFactory = new SharpDX.DXGI.Factory2(false);
            //                }
            //#else
            //                dxgiFactory = new SharpDX.DXGI.Factory2(false);
            //#endif
            //                //CreateSwapChainForComposition
            //                _d3D11Device = null;
            //                for (var i = 0; i < dxgiFactory.GetAdapterCount(); i++)
            //                {
            //                    try
            //                    {
            //                        var adapter = dxgiFactory.GetAdapter(i);
            //                        _d3D11Device = new SharpDX.Direct3D11.Device(adapter, deviceCreationFlags);
            //                        adapter.Dispose();
            //                        adapter = null;
            //                        break;
            //                    }
            //                    catch (SharpDXException)
            //                    {
            //                    }
            //                }

            //                if (_d3D11Device is null)
            //                {
            //                    throw new VLCException("Could not create Direct3D11 device : No compatible adapter found.");
            //                }

            //                var device = _d3D11Device.QueryInterface<SharpDX.DXGI.Device1>();

            //                //Create the swapchain
            //                var swapChainDescription = new SharpDX.DXGI.SwapChainDescription1
            //                {
            //                    Width = (int)(_panel.ActualWidth * _panel.CompositionScaleX),
            //                    Height = (int)(_panel.ActualHeight * _panel.CompositionScaleY),
            //                    Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
            //                    Stereo = false,
            //                    SampleDescription =
            //                    {
            //                        Count = 1,
            //                        Quality = 0
            //                    },
            //                    Usage = Usage.RenderTargetOutput,
            //                    BufferCount = 2,
            //                    SwapEffect = SwapEffect.FlipSequential,
            //                    Flags = SwapChainFlags.None,
            //                    AlphaMode = AlphaMode.Unspecified
            //                };

            //                _swapChain = new SharpDX.DXGI.SwapChain1(dxgiFactory, _d3D11Device, ref swapChainDescription);
            //                dxgiFactory.Dispose();
            //                dxgiFactory = null;

            //                device.MaximumFrameLatency = 1;

            //                using (var panelNative = ComObject.As<ISwapChainPanelNative>(_panel))
            //                {
            //                    panelNative.SwapChain = _swapChain;
            //                }

            //                // This is necessary so we can call Trim() on suspend
            //                _device3 = device.QueryInterface<SharpDX.DXGI.Device3>();
            //                if (_device3 == null)
            //                {
            //                    throw new VLCException("Failed to query interface \"Device3\"");
            //                }

            //                device.Dispose();
            //                device = null;

            //                _swapChain2 = _swapChain.QueryInterface<SharpDX.DXGI.SwapChain2>();
            //                if (_swapChain2 == null)
            //                {
            //                    throw new VLCException("Failed to query interface \"SwapChain2\"");
            //                }

            //                UpdateScale();
            //                UpdateSize();
            //                _loaded = true;
            // }
            //catch (Exception ex)
            //{
            //    //DestroySwapChain();
            //    //if (ex is SharpDXException)
            //    //{
            //    //    throw new VLCException("SharpDX operation failed, see InnerException for details", ex);
            //    //}

            //    throw;
            //}
        }

        /// <summary>
        /// Destroys the SwapChain and all related instances.
        /// </summary>
        void DestroySwapChain()
        {
            //_swapChain2?.Dispose();
            //_swapChain2 = null;

            //_device3?.Dispose();
            //_device3 = null;

            //if (_panel != null)
            //{
            //    using (var panelNative = ComObject.As<ISwapChainPanelNative>(_panel))
            //    {
            //        panelNative.SwapChain = null;
            //    }
            //}

            //_swapChain?.Dispose();
            //_swapChain = null;

            //_deviceContext?.Dispose();
            //_deviceContext = null;

            //_d3D11Device?.Dispose();
            //_d3D11Device = null;

            _loaded = false;
        }

   
        /// <summary>
        /// Associates width/height private data into the SwapChain, so that VLC knows at which size to render its video.
        /// </summary>
        void UpdateSize()
        {
            //if (_panel is null || _swapChain is null || _swapChain.IsDisposed)
            //    return;

            //var width = IntPtr.Zero;
            //var height = IntPtr.Zero;

            //try
            //{
            //    width = Marshal.AllocHGlobal(sizeof(int));
            //    height = Marshal.AllocHGlobal(sizeof(int));

            //    var w = (int)(_panel.ActualWidth * _panel.CompositionScaleX);
            //    var h = (int)(_panel.ActualHeight * _panel.CompositionScaleY);

            //    Marshal.WriteInt32(width, w);
            //    Marshal.WriteInt32(height, h);

            //    _swapChain.SetPrivateData(SWAPCHAIN_WIDTH, sizeof(int), width);
            //    _swapChain.SetPrivateData(SWAPCHAIN_HEIGHT, sizeof(int), height);
            //}
            //finally
            //{
            //    Marshal.FreeHGlobal(width);
            //    Marshal.FreeHGlobal(height);
            //}
        }

        /// <summary>
        /// Updates the MatrixTransform of the SwapChain.
        /// </summary>
        void UpdateScale()
        {
            //if (_panel is null)
            //    return;
            //_swapChain2!.MatrixTransform = new RawMatrix3x2 { M11 = 1.0f / _panel.CompositionScaleX, M22 = 1.0f / _panel.CompositionScaleY };
        }

        /// <summary>
        /// When the app is suspended, UWP apps should call Trim so that the DirectX data is cleaned.
        /// </summary>
        void Trim()
        {
            //_device3?.Trim();
        }

        /// <summary>
        /// When the media player is attached to the view.
        /// </summary>
        void Attach()
        {
        }

        /// <summary>
        /// When the media player is detached from the view.
        /// </summary>
        void Detach()
        {
        }


        /// <summary>
        /// Identifies the <see cref="MediaPlayer"/> dependency property.
        /// </summary>
        public static DependencyProperty MediaPlayerProperty { get; } = DependencyProperty.Register(nameof(MediaPlayer), typeof(MediaPlayer),
            typeof(VideoView), new PropertyMetadata(null, OnMediaPlayerChanged));
        /// <summary>
        /// MediaPlayer object connected to the view
        /// </summary>
        public MediaPlayer? MediaPlayer
        {
            get => (MediaPlayer?)GetValue(MediaPlayerProperty);
            set => SetValue(MediaPlayerProperty, value);
        }

        private static void OnMediaPlayerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var videoView = (VideoView)d;
            videoView.Detach();
            if (e.NewValue != null)
            {
                videoView.Attach();
            }
        }
    }

//#if WINUI
//    [Guid("63aad0b8-7c24-40ff-85a8-640d944cc325")]
//    internal class ISwapChainPanelNative : SharpDX.DXGI.ISwapChainPanelNative
//    {
//        public ISwapChainPanelNative(IntPtr nativePtr) : base(nativePtr) { }
//    }
//#endif
}
