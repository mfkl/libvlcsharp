using System;
using System.Windows.Forms;
using TerraFX.Interop;
using System.Runtime.InteropServices;

using static TerraFX.Interop.Windows;
using static TerraFX.Interop.D3D_DRIVER_TYPE;
using static TerraFX.Interop.D3D_FEATURE_LEVEL;
using static TerraFX.Interop.DXGI_ADAPTER_FLAG;


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

        static void ThrowIfFailed(HRESULT hr)
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

        static unsafe bool SupportsRequiredDirect3DVersion(IDXGIAdapter1* adapter)
        {
            var featureLevel = D3D_FEATURE_LEVEL_11_0;
            return SUCCEEDED(D3D11CreateDevice((IDXGIAdapter*)adapter, D3D_DRIVER_TYPE_HARDWARE, Software: IntPtr.Zero, Flags: 0, &featureLevel, FeatureLevels: 1, D3D11_SDK_VERSION, ppDevice: null, pFeatureLevel: null, ppImmediateContext: null));
        }

        static IDXGIAdapter1* GetHardwareAdapter(IDXGIFactory1* pFactory)
        {
            IDXGIAdapter1* adapter;

            for (var adapterIndex = 0u; DXGI_ERROR_NOT_FOUND != pFactory->EnumAdapters1(adapterIndex, &adapter); ++adapterIndex)
            {
                DXGI_ADAPTER_DESC1 desc;
                _ = adapter->GetDesc1(&desc);

                if ((desc.Flags & (uint)DXGI_ADAPTER_FLAG_SOFTWARE) != 0)
                {
                    // Don't select the Basic Render Driver adapter.
                    // If you want a software adapter, pass in "/warp" on the command line.
                    continue;
                }

                // Check to see if the adapter supports the required Direct3D version, but don't create the
                // actual device yet.
                if (SupportsRequiredDirect3DVersion(adapter))
                {
                    break;
                }
            }

            return adapter;
        }

        static void InitializeDirect3D()
        {
            IDXGIFactory1* dxgiFactory;

            var iidd = IID_IDXGIFactory1;
            ThrowIfFailed(CreateDXGIFactory1(&iidd, (void**)&dxgiFactory));

            _adapter = GetHardwareAdapter(dxgiFactory);

            //dxgiFactory->Release();

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

            //D3D11CreateDevice((IDXGIAdapter*)DxgiAdapter, D3D_DRIVER_TYPE_HARDWARE, Software: IntPtr.Zero, Flags: 0, &featureLevel, FeatureLevels: 1, D3D11_SDK_VERSION, &d3dDevice, pFeatureLevel: null, &immediateContext));

            var featureLevel = D3D_FEATURE_LEVEL_11_0;


            fixed (IDXGISwapChain** swapchain = &_swapchain)
            fixed (ID3D11Device** device = &_d3dDevice)
            fixed (ID3D11DeviceContext** context = &_d3dctx)
            { 
                ThrowIfFailed(D3D11CreateDeviceAndSwapChain((IDXGIAdapter*)_adapter, 
                        D3D_DRIVER_TYPE_HARDWARE,
                        Software: IntPtr.Zero,
                        Flags: creationFlags,
                        null, //&featureLevel,
                        0, //FeatureLevels: 1, 
                        D3D11_SDK_VERSION,
                        &desc,
                        swapchain,
                        device,
                        null,
                        context));
            }

            ID3D10Multithread* pMultithread;
            var iid = IID_ID3D10Multithread;

            ThrowIfFailed(_d3dDevice->QueryInterface(&iid, (void**)&pMultithread));
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
                ThrowIfFailed(D3D11CreateDevice((IDXGIAdapter*)_adapter,
                      D3D_DRIVER_TYPE_HARDWARE,
                      IntPtr.Zero,
                      creationFlags | (uint)D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_VIDEO_SUPPORT, /* needed for hardware decoding */
                      &featureLevel, 1,
                      D3D11_SDK_VERSION,
                      device, null, context));
            }

            using ComPtr<ID3D11Resource> pBackBuffer = null;

            iid = IID_ID3D11Texture2D;
            ThrowIfFailed(_swapchain->GetBuffer(0, &iid, (void**)pBackBuffer.GetAddressOf()));

            fixed (ID3D11RenderTargetView** swapchainRenderTarget = &_swapchainRenderTarget)
                ThrowIfFailed(_d3dDevice->CreateRenderTargetView(pBackBuffer.Get(), null, swapchainRenderTarget));

            pBackBuffer.Dispose();

            fixed (ID3D11RenderTargetView** swapchainRenderTarget = &_swapchainRenderTarget)
                _d3dctx->OMSetRenderTargets(1, swapchainRenderTarget, null);

            ID3DBlob* VS, PS, pErrBlob;
            
            using ComPtr<ID3DBlob> vertexShaderBlob = null;

            fixed (void* shader = shaderStr)
            fixed (char* fileName = @"C:\Users\Martin\Projects\LibVLCSharp\samples\LibVLCSharp.CustomRendering.Direct3D11\bin\Debug\net5.0-windows\shader.hlsl")
            //fixed (byte* entrypoint = Encoding.ASCII.GetBytes("VShader"))
            //fixed (byte* target = Encoding.ASCII.GetBytes("vs_4_0"))
            {
                var entrypoint = 0x00006E69614D5356;    // VSMain
                var target = 0x0000305F345F7376;        // vs_4_0

                ThrowIfFailed(D3DCompileFromFile((ushort*)fileName, null, null, (sbyte*)&entrypoint, (sbyte*)&target, 1 << 0, 0, &VS, &pErrBlob));

                //D3DCompile(shader, (nuint)shaderStr.Length, null, null, null, (sbyte*)&entrypoint, (sbyte*)&target, 1 << 0, 0, &VS, &pErrBlob);

                //ThrowIfFailed(D3DCompile(shader, (nuint)shaderStr.Length, null, null, null, (sbyte*)&entrypoint, (sbyte*)&target, 1 << 0, 0, &VS, &pErrBlob));
                //var hr = pErrBlob->GetBufferPointer();

            }


        }

        static string shaderStr = @"\
Texture2D shaderTexture;\n\
SamplerState samplerState;\n\
struct PS_INPUT\n\
{\n\
    float4 position     : SV_POSITION;\n\
    float4 textureCoord : TEXCOORD0;\n\
};\n\
\n\
float4 PShader(PS_INPUT In) : SV_TARGET\n\
{\n\
    return shaderTexture.Sample(samplerState, In.textureCoord);\n\
}\n\
\n\
struct VS_INPUT\n\
{\n\
    float4 position     : POSITION;\n\
    float4 textureCoord : TEXCOORD0;\n\
};\n\
\n\
struct VS_OUTPUT\n\
{\n\
    float4 position     : SV_POSITION;\n\
    float4 textureCoord : TEXCOORD0;\n\
};\n\
\n\
VS_OUTPUT VShader(VS_INPUT In)\n\
{\n\
    return In;\n\
}\n\
";
        private static IDXGIAdapter1* _adapter;
    }
}