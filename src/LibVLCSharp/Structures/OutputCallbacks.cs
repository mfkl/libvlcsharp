using System;
using System.Runtime.InteropServices;

namespace LibVLCSharp.Structures
{
    /// <summary>
    /// Device configuration setup for ouput callbacks
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct SetupDeviceConfig
    {
        /// <summary>
        /// set to true if D3D11_CREATE_DEVICE_VIDEO_SUPPORT is needed for D3D11
        /// </summary>
        public readonly bool HardwareDecoding;
    }

    /// <summary>
    /// Information about the device
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct SetupDeviceInfo
    {
        /// <summary>
        /// D3D11 info
        /// </summary>
        [FieldOffset(0)]
        public readonly SetupDeviceInfoD3D11 D3D11;

        /// <summary>
        /// D3D9 info
        /// </summary>
        [FieldOffset(0)]
        public readonly SetupDeviceInfoD3D9 D3D9;
    }

    /// <summary>
    /// Struct for d3d11 setup device info.
    /// </summary>
    public readonly struct SetupDeviceInfoD3D11
    {
        /// <summary>
        /// ID3D11DeviceContext
        /// </summary>
        public readonly IntPtr deviceContext;
    }

    /// <summary>
    /// Struct for d3d9 setup device info.
    /// </summary>
    public readonly struct SetupDeviceInfoD3D9
    {
        /// <summary>
        /// IDirect3D9
        /// </summary>
        public readonly IntPtr Device;

        /// <summary>
        /// Adapter to use with the IDirect3D9*
        /// </summary>
        public readonly int Adapter;
    }

    /// <summary>
    /// Output callback render configuration
    /// </summary>
    public readonly struct RenderConfig
    {
        /// <summary>
        /// rendering video width in pixel
        /// </summary>
        public readonly uint Width;

        /// <summary>
        /// rendering video height in pixel
        /// </summary>
        public readonly uint Height;

        /// <summary>
        /// rendering video bit depth in bits per channel
        /// </summary>
        public readonly uint BitDepth;

        /// <summary>
        /// video is full range or studio/limited range
        /// </summary>
        public readonly bool FullRange;

        /// <summary>
        /// video color space
        /// </summary>
        public readonly ColorSpace ColorSpace;

        /// <summary>
        /// video color primaries
        /// </summary>
        public readonly ColorPrimaries Primaries;

        /// <summary>
        /// video transfer function
        /// </summary>

        public readonly TransferFunction Transfer;

        /// <summary>
        /// device used for rendering, IDirect3DDevice9* for D3D9
        /// </summary>
        public readonly IntPtr Device;
    }

    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct OutputConfig
    {
        /// <summary>
        /// the rendering DXGI_FORMAT for d3d11
        /// </summary>
        [FieldOffset(0)]
        public int DxgiFormat;

        /// <summary>
        /// the rendering D3DFORMAT for d3d9
        /// </summary>
        [FieldOffset(0)]
        public uint D3d9Format;

        /// <summary>
        /// the rendering GLint GL_RGBA or GL_RGB for opengl and opengles2
        /// </summary>
        [FieldOffset(0)]
        public int OpenGLFormat;

        /// <summary>
        /// unused
        /// </summary>
        [FieldOffset(0)]
        public IntPtr Surface;

        /// <summary>
        /// video is full range or studio/limited range
        /// </summary>
        [FieldOffset(4)]
        public bool FullRange;

        /// <summary>
        /// video color space
        /// </summary>
        [FieldOffset(8)]
        public ColorSpace ColorSpace;

        /// <summary>
        /// video color primaries
        /// </summary>
        [FieldOffset(12)]
        public ColorPrimaries ColorPrimaries;

        /// <summary>
        /// video transfer function
        /// </summary>
        [FieldOffset(16)]
        public TransferFunction TransferFunction;
    }

    /// <summary>
    /// Metadata for HDR10 medias
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct D3DHDR10Metadata
    {
        /// <summary>
        /// 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ushort[] RedPrimary;

        /// <summary>
        /// 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ushort[] GreenPrimary;

        /// <summary>
        /// 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ushort[] BluePrimary;

        /// <summary>
        /// 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public ushort[] WhitePoint;

        /// <summary>
        /// 
        /// </summary>
        public uint MaxMasteringLuminance;

        /// <summary>
        /// 
        /// </summary>
        public uint MinMasteringLuminance;

        /// <summary>
        /// 
        /// </summary>
        public ushort MaxContentLightLevel;

        /// <summary>
        /// 
        /// </summary>
        public ushort MaxFrameAverageLightLevel;
    }

    /// <summary>
    /// Enumeration of the Video engine to be used on output.
    /// </summary>
    public enum VideoEngine
    {
        /// <summary>
        /// Disable rendering engine
        /// </summary>
        Disable,

        /// <summary>
        /// OpenGL rendering engine
        /// </summary>
        OpenGL,

        /// <summary>
        /// OpenGLES2 rendering engine
        /// </summary>
        OpenGLES2,

        /// <summary>
        /// Direct3D11 rendering engine
        /// </summary>
        D3D11,

        /// <summary>
        /// Direct3D9 rendering engine
        /// </summary>
        D3D9
    }

    /// <summary>
    /// Enumeration of the Video color spaces.
    /// </summary>
    public enum ColorSpace
    {
        /// <summary>
        /// Rec. 601
        /// </summary>
        BT601 = 1,

        /// <summary>
        /// Rec.709
        /// </summary>
        BT709 = 2,

        /// <summary>
        /// Rec. 2020
        /// </summary>
        BT2020 = 3
    }

    /// <summary>
    /// Enumeration of the Video color primaries.
    /// </summary>
    public enum ColorPrimaries
    {
        /// <summary>
        /// 
        /// </summary>
        BT601_525 = 1,

        /// <summary>
        /// 
        /// </summary>
        BT601_625 = 2,

        /// <summary>
        /// 
        /// </summary>
        BT709 = 3,

        /// <summary>
        /// 
        /// </summary>
        BT2020 = 4,

        /// <summary>
        /// 
        /// </summary>
        DCI_P3 = 5,

        /// <summary>
        /// 
        /// </summary>
        BT470_M = 6
    }

    /// <summary>
    /// Enumeration of the Video transfer functions.
    /// </summary>
    public enum TransferFunction
    {
        /// <summary>
        /// 
        /// </summary>
        LINEAR = 1,

        /// <summary>
        /// 
        /// </summary>
        SRGB = 2,

        /// <summary>
        /// 
        /// </summary>
        BT470_BG = 3,

        /// <summary>
        /// 
        /// </summary>
        BT470_M = 4,

        /// <summary>
        /// 
        /// </summary>
        BT709 = 5,

        /// <summary>
        /// 
        /// </summary>
        PQ = 6,

        /// <summary>
        /// 
        /// </summary>
        SMPTE_240 = 7,

        /// <summary>
        /// 
        /// </summary>
        HLG = 8
    }

    /// <summary>
    /// 
    /// </summary>
    public enum MetadataType
    {
        /// <summary>
        /// 
        /// </summary>
        FrameHDR10
    }
}
