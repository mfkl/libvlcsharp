using System;
using System.Windows.Forms;
using TerraFX.Interop;
using System.Runtime.InteropServices;

using static TerraFX.Interop.Windows;
using static TerraFX.Interop.D3D_DRIVER_TYPE;

namespace LibVLCSharp.CustomRendering.Direct3D11
{
    unsafe class Program
    {
        static Form form;
        static IDXGISwapChain* _swapchain;
        static ID3D11RenderTargetView* _swapchainRenderTarget;

        static ID3D11Device* _d3dDevice;
        static ID3D11DeviceContext* _d3dctx;

        const int WIDTH = 1500;
        const int HEIGHT = 900;

        static ID3D11Device* _d3deviceVLC;
        static ID3D11DeviceContext* _d3dctxVLC;

        static void ThrowIfFailed(string methodName, HRESULT hr)
        {
            if (FAILED(hr))
            {
                Marshal.ThrowExceptionForHR(hr);
            }
        }

        static void Main()
        {
            CreateWindow();
            InitializeDirect3D();
        }

        static void CreateWindow()
        {
            form = new Form() { Width = WIDTH, Height = HEIGHT };
            form.Show();
            form.Resize += Form_Resize;
        }

        static void Form_Resize(object sender, EventArgs e)
        {
        }

        static void InitializeDirect3D()
        {
            var desc = new DXGI_SWAP_CHAIN_DESC
            {
                BufferDesc = new DXGI_MODE_DESC
                {
                    Width = WIDTH,
                    Height = HEIGHT,
                    Format = DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_UNORM,
                },
                SampleDesc = new DXGI_SAMPLE_DESC
                {
                    Count = 1
                },
                BufferCount = 1,
                Windowed = TRUE,
                OutputWindow = form.Handle,
                BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT,
                Flags = (uint)DXGI_SWAP_CHAIN_FLAG.DXGI_SWAP_CHAIN_FLAG_ALLOW_MODE_SWITCH
            };

            uint creationFlags = 0;
#if DEBUG
            creationFlags |= (uint)D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_DEBUG;
#endif

            fixed (IDXGISwapChain** swapchain = &_swapchain)
            fixed (ID3D11Device** device = &_d3dDevice)
            fixed (ID3D11DeviceContext** context = &_d3dctx)
            { 
                ThrowIfFailed(nameof(D3D11CreateDeviceAndSwapChain), 
                    D3D11CreateDeviceAndSwapChain(null, 
                        D3D_DRIVER_TYPE_HARDWARE, 
                        IntPtr.Zero,
                        creationFlags, 
                        null,
                        0, 
                        D3D11_SDK_VERSION,
                        &desc,
                        swapchain,
                        device,
                        null,
                        context));
            }

            ID3D10Multithread* pMultithread;
            var iid = IID_ID3D10Multithread;

            ThrowIfFailed(nameof(ID3D11Device.QueryInterface), _d3dDevice->QueryInterface(&iid, (void**)&pMultithread));
            pMultithread->SetMultithreadProtected(TRUE);
            pMultithread->Release();

            var viewport = new D3D11_VIEWPORT 
            {
                Height = HEIGHT,
                Width = WIDTH
            };

            _d3dctx->RSSetViewports(1, &viewport);

            fixed (ID3D11Device** device = &_d3deviceVLC)
            fixed (ID3D11DeviceContext** context = &_d3dctxVLC)
            {
                ThrowIfFailed(nameof(D3D11CreateDevice),
                D3D11CreateDevice(null,
                      D3D_DRIVER_TYPE_HARDWARE,
                      IntPtr.Zero,
                      creationFlags | (uint)D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_VIDEO_SUPPORT, /* needed for hardware decoding */
                      null, 0,
                      D3D11_SDK_VERSION,
                      device, null, context));
            }

            using ComPtr<ID3D11Resource> pBackBuffer = null;

            iid = IID_ID3D11Texture2D;
            ThrowIfFailed(nameof(IDXGISwapChain.GetBuffer), _swapchain->GetBuffer(0, &iid, (void**)pBackBuffer.GetAddressOf()));

            fixed (ID3D11RenderTargetView** swapchainRenderTarget = &_swapchainRenderTarget)
                ThrowIfFailed(nameof(ID3D11Device.CreateRenderTargetView), _d3dDevice->CreateRenderTargetView(pBackBuffer.Get(), null, swapchainRenderTarget));

            pBackBuffer.Dispose();

            fixed (ID3D11RenderTargetView** swapchainRenderTarget = &_swapchainRenderTarget)
                _d3dctx->OMSetRenderTargets(1, swapchainRenderTarget, null);
        }
    }
}