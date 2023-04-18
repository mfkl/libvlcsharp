using System;
using System.Runtime.InteropServices;
using LibVLCSharp;
using Windows.Win32.System.Threading;
using Windows.Win32.Graphics.Dxgi;
using Windows.Win32.Graphics.Dxgi.Common;
using Windows.Win32.Graphics.Direct3D11;
using Windows.Win32.Graphics.Direct3D;
using Windows.Win32.Foundation;
using Windows.Win32;
using Windows.Win32.Graphics.Direct3D10;
using Windows.System.Profile;

using static Windows.Win32.PInvoke;
using Windows.Win32.System.Com;
using System.Runtime.CompilerServices;

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

        IDXGISwapChain1* _swapchain;
        //ID3D11RenderTargetView* _swapchainRenderTarget;

        //ID3D11Device* _d3dDevice;
        //ID3D11DeviceContext* _d3dctx;

        //int WIDTH = 1500;
        //int HEIGHT = 900;

        ID3D11Device* _d3deviceVLC;
        ID3D11DeviceContext* _d3dctxVLC;

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

        void ThrowIfFailed(HRESULT hr)
        {
            if (hr.Failed)
            {
                Marshal.ThrowExceptionForHR(hr);
            }
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

            D3D11_CREATE_DEVICE_FLAG creationFlags = 0;
#if DEBUG
            creationFlags |= D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_DEBUG;
#endif
            fixed (ID3D11Device** device = &_d3deviceVLC)
            fixed (ID3D11DeviceContext** context = &_d3dctxVLC)
            {
                ThrowIfFailed(D3D11CreateDevice(null,
                      D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_HARDWARE,
                      HINSTANCE.Null,
                      creationFlags | D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_VIDEO_SUPPORT, /* needed for hardware decoding */
                      null, 0,
                      D3D11_SDK_VERSION,
                      device, null, context));
            }

            var desc = new DXGI_SWAP_CHAIN_DESC1
            {
                Width = (uint)(_panel.ActualWidth * _panel.CompositionScaleX),
                Height = (uint)(_panel.ActualHeight * _panel.CompositionScaleY),
                Format = DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM,
                Stereo = false,
                SampleDesc = new DXGI_SAMPLE_DESC { Count = 1, Quality = 0 },
                BufferUsage = DXGI_USAGE.DXGI_USAGE_RENDER_TARGET_OUTPUT,
                BufferCount = 2,
                SwapEffect = DXGI_SWAP_EFFECT.DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL,
                AlphaMode = DXGI_ALPHA_MODE.DXGI_ALPHA_MODE_UNSPECIFIED
            };

            IUnknown* res;
            var iunknownGuid = typeof(IUnknown).GUID;
            _d3deviceVLC->QueryInterface(&iunknownGuid, (void**)&res);

            var factory = new IDXGIFactory2();
            //fixed (DXGI_SWAP_CHAIN_DESC1* descc = &desc)
            fixed (IDXGISwapChain1** swapchain = &_swapchain)
            { 
                factory.CreateSwapChainForComposition(res, (DXGI_SWAP_CHAIN_DESC1*)Unsafe.AsPointer(ref desc), null, swapchain);
            }

            factory.Release();

            //                device.MaximumFrameLatency = 1;
            //var sw = (Windows.Win32.System.WinRT.Xaml.ISwapChainPanelNative)_panel;

            //fixed (* sw = _panel)
            //{

            //}
            //using (var panelNative = ComObject.As<>(_panel))
            //{
            //    panelNative.SwapChain = _swapChain;
            //}

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
