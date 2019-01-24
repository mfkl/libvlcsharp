/*****************************************************************************
 * Copyright © 2013-2014 VideoLAN
 *
 * Authors: Kellen Sunderland
 *
 * This program is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation; either version 2.1 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin Street, Fifth Floor, Boston MA 02110-1301, USA.
 *****************************************************************************/

#include <wrl.h>
#include <wrl/client.h>

#include "windows.ui.xaml.media.dxinterop.h"

#include "DirectXManager.h"
#include <collection.h>

 //HACK HACK HACK
using namespace Windows::Graphics::Display;
using namespace LibVLCSharp_UWP;

DirectXManager::DirectXManager()
{
}

void DirectXManager::CheckDXOperation(HRESULT hr, Platform::String^ message) {
	if (hr != S_OK) {
		throw ref new Platform::Exception(hr, message);
	}
}

void DirectXManager::SetupSwapChainPanel(SwapChainPanel^ panel) {
	HRESULT hr;
	ComPtr<IDXGIFactory2> dxgiFactory;
	ComPtr<IDXGIAdapter> dxgiAdapter;
	ComPtr<IDXGIDevice1> dxgiDevice;

	UINT i_factoryFlags = 0;
#if defined(_DEBUG)
	i_factoryFlags |= DXGI_CREATE_FACTORY_DEBUG;
#endif
	hr = CreateDXGIFactory2(i_factoryFlags, __uuidof(IDXGIFactory2), &dxgiFactory);
#if defined(_DEBUG)
	if (FAILED(hr))
	{
		i_factoryFlags &= ~DXGI_CREATE_FACTORY_DEBUG;
		hr = CreateDXGIFactory2(i_factoryFlags, __uuidof(IDXGIFactory2), &dxgiFactory);
	}
#endif
	CheckDXOperation(hr, "Could not create DXGI factory");

	UINT creationFlags = D3D11_CREATE_DEVICE_BGRA_SUPPORT | D3D11_CREATE_DEVICE_VIDEO_SUPPORT;
#if defined(_DEBUG)
	if (i_factoryFlags & DXGI_CREATE_FACTORY_DEBUG)
		creationFlags |= D3D11_CREATE_DEVICE_DEBUG;
#endif

	cp_d3dDevice = nullptr;
	UINT i_adapter = 0;
	while (cp_d3dDevice == nullptr)
	{
		hr = dxgiFactory->EnumAdapters(i_adapter++, &dxgiAdapter);
		if (FAILED(hr))
		{
			if (creationFlags & D3D11_CREATE_DEVICE_VIDEO_SUPPORT)
			{
				/* try again without this flag */
				i_adapter = 0;
				creationFlags &= ~D3D11_CREATE_DEVICE_VIDEO_SUPPORT;
				continue; //Try again with the new flags
			}
			else
				break; /* no more flags to remove */
		}

		hr = D3D11CreateDevice(
			dxgiAdapter.Get(),
			D3D_DRIVER_TYPE_UNKNOWN,
			nullptr,
			creationFlags,
			NULL,
			0,
			D3D11_SDK_VERSION,
			&cp_d3dDevice,
			nullptr,
			&cp_d3dContext
		);
		if (FAILED(hr))
			cp_d3dDevice = nullptr;
	}
	CheckDXOperation(hr, "Could not D3D11CreateDevice");
	CheckDXOperation(cp_d3dDevice.As(&dxgiDevice), "Could not transform to DXGIDevice");
	//Create the swapchain
	DXGI_SWAP_CHAIN_DESC1 swapChainDesc = { 0 };
	swapChainDesc.Width = (UINT)(panel->ActualWidth * panel->CompositionScaleX);
	swapChainDesc.Height = (UINT)(panel->ActualHeight * panel->CompositionScaleY);
	swapChainDesc.Format = DXGI_FORMAT_B8G8R8A8_UNORM;
	swapChainDesc.Stereo = false;
	swapChainDesc.SampleDesc.Count = 1;
	swapChainDesc.SampleDesc.Quality = 0;
	swapChainDesc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
	swapChainDesc.BufferCount = 2;
	swapChainDesc.SwapEffect = DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL;
	swapChainDesc.Flags = 0;
	swapChainDesc.AlphaMode = DXGI_ALPHA_MODE_UNSPECIFIED;

	hr = dxgiFactory->CreateSwapChainForComposition(
		cp_d3dDevice.Get(),
		&swapChainDesc,
		nullptr,
		&cp_swapChain
	);
	CheckDXOperation(hr, "Could not create swapChain");
	CheckDXOperation(dxgiDevice->SetMaximumFrameLatency(1), "Could not set maximum Frame Latency");

	//TODO: perform the next 2 calls on the UI thread
	ComPtr<ISwapChainPanelNative> panelNative;
	hr = reinterpret_cast<IUnknown*>(panel)->QueryInterface(IID_PPV_ARGS(&panelNative));
	CheckDXOperation(hr, "Could not initialise the native panel");

	// Associate swap chain with SwapChainPanel.  This must be done on the UI thread.
	CheckDXOperation(panelNative->SetSwapChain(cp_swapChain.Get()), "Could not associate the swapChain");

	// This is necessary so we can call Trim() on suspend
	hr = dxgiDevice.As(&cp_dxgiDev3);
	CheckDXOperation(hr, "Failed to get the DXGIDevice3 from Dxgidevice1");

	hr = cp_swapChain.As(&cp_swapChain2);
	CheckDXOperation(hr, "Failed to get IDXGISwapChain2 from IDXGISwapChain1");
	UpdateScale(panel->CompositionScaleX, panel->CompositionScaleY);
	swapChainPanel = panel;
}

void DirectXManager::InitializeHack(Platform::Array<Platform::String^>^* argv, uint32 nbArgs)
{
	SetupSwapChainPanel(swapChainPanel);

	UpdateSize(swapChainPanel->ActualWidth * swapChainPanel->CompositionScaleX,
		swapChainPanel->ActualHeight * swapChainPanel->CompositionScaleY);

	char ptr_d3dcstring[64];
	sprintf_s(ptr_d3dcstring, "--winrt-d3dcontext=0x%p", cp_d3dContext);
	//argv[nbArgs++] = _strdup(ptr_d3dcstring);

	char ptr_scstring[64];
	sprintf_s(ptr_scstring, "--winrt-swapchain=0x%p", cp_swapChain);
	//argv[nbArgs++] = _strdup(ptr_scstring);
}

static const GUID SWAPCHAIN_WIDTH = { 0xf1b59347, 0x1643, 0x411a,{ 0xad, 0x6b, 0xc7, 0x80, 0x17, 0x7a, 0x6, 0xb6 } };
static const GUID SWAPCHAIN_HEIGHT = { 0x6ea976a0, 0x9d60, 0x4bb7,{ 0xa5, 0xa9, 0x7d, 0xd1, 0x18, 0x7f, 0xc9, 0xbd } };

void DirectXManager::UpdateSize(float x, float y)
{
	m_width = (uint32_t)x;
	m_height = (uint32_t)y;
	cp_swapChain->SetPrivateData(SWAPCHAIN_WIDTH, sizeof(uint32_t), &m_width);
	cp_swapChain->SetPrivateData(SWAPCHAIN_HEIGHT, sizeof(uint32_t), &m_height);
}

void DirectXManager::UpdateScale(float scaleX, float scaleY)
{
	DXGI_MATRIX_3X2_F inverseScale = { 0 };
	inverseScale._11 = 1.0f / scaleX;
	inverseScale._22 = 1.0f / scaleY;
	cp_swapChain2->SetMatrixTransform(&inverseScale);
}

void DirectXManager::Trim()
{
	if (cp_dxgiDev3 != nullptr)
		cp_dxgiDev3->Trim();
}
