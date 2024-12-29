using SharpGen.Runtime;
using Vortice.DCommon;
using Vortice.Direct2D1;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;
using FeatureLevel = Vortice.Direct3D.FeatureLevel;

namespace GLibTest
{
	public partial class D2DControl : UserControl
	{
		private ID3D11Device? _d3dDevice;
		private ID2D1Factory1? _direct2DFactory;
		private ID2D1DeviceContext? _d2dDeviceContext;
		private IDXGISwapChain1? _swapChain;
		private ID2D1Bitmap1? _renderTarget;
		private bool _resizing = false;

		// Vsync Property
		private bool _vsync = false;
		public bool VSync
		{
			get
			{
				return _vsync;
			}
			set
			{
				_vsync = value;
				// Recreate the swap chain to apply the new VSync setting
				if (_d3dDevice != null)
				{
					_renderTarget?.Dispose();
					if (_d2dDeviceContext != null) _d2dDeviceContext.Target = null;
					_swapChain?.Dispose();
					CreateSwapChain(_d3dDevice);
					CreateRenderTarget();
				}
			}
		}


		public delegate void RenderHandler(object sender, ID2D1DeviceContext g);
		public event RenderHandler? OnRendering;

		public D2DControl()
		{
			InitializeComponent();
			SetControlStyles();
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			InitializeDirect2DResources();
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			DisposeResources();
			base.OnHandleDestroyed(e);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			ResizeRenderTarget();
			Invalidate(); // Trigger a repaint after resizing
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			// Override to avoid flickering; background will be cleared during rendering
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (_d2dDeviceContext == null) return;

			_d2dDeviceContext.BeginDraw();

			// Trigger custom rendering logic
			OnRendering?.Invoke(this, _d2dDeviceContext);

			_d2dDeviceContext.EndDraw();
			_swapChain?.Present(VSync ? 1u : 0, VSync ? PresentFlags.None : PresentFlags.AllowTearing);
		}

		private void SetControlStyles()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
		}

		private void InitializeDirect2DResources()
		{
			// Create Direct3D device
			FeatureLevel[] featureLevels = [
				FeatureLevel.Level_11_1,
				FeatureLevel.Level_11_0,
				FeatureLevel.Level_10_1,
				FeatureLevel.Level_10_0,
				FeatureLevel.Level_9_3,
				FeatureLevel.Level_9_2,
				FeatureLevel.Level_9_1
			];

			_d3dDevice = D3D11.D3D11CreateDevice(
				DriverType.Hardware,
				DeviceCreationFlags.Singlethreaded | DeviceCreationFlags.BgraSupport,
				featureLevels);

			using IDXGIDevice dxgiDevice = _d3dDevice.QueryInterface<IDXGIDevice>();
			// Create Direct2D factory and device
			_direct2DFactory = D2D1.D2D1CreateFactory<ID2D1Factory1>(FactoryType.SingleThreaded);
			using ID2D1Device d2dDevice = _direct2DFactory.CreateDevice(dxgiDevice);
			_d2dDeviceContext = d2dDevice.CreateDeviceContext(DeviceContextOptions.None);

			CreateSwapChain(_d3dDevice);
			CreateRenderTarget();
		}

		private void CreateSwapChain(ID3D11Device device)
		{
			SwapChainDescription1 swapChainDesc = new SwapChainDescription1
			{
				Width = 0,
				Height = 0,
				Format = Format.B8G8R8A8_UNorm,
				SampleDescription = new SampleDescription(1, 0),
				BufferUsage = Usage.RenderTargetOutput,
				BufferCount = 2,
				Scaling = Scaling.None,
				SwapEffect = SwapEffect.FlipSequential,
				Flags = VSync ? SwapChainFlags.None : SwapChainFlags.AllowTearing
			};

			using IDXGIFactory2 dxgiFactory = device.QueryInterface<IDXGIDevice>()
										  .GetAdapter()
										  .GetParent<IDXGIFactory2>();
			_swapChain = dxgiFactory.CreateSwapChainForHwnd(device, Handle, swapChainDesc);
		}

		private void CreateRenderTarget()
		{
			if (_d2dDeviceContext == null || _swapChain == null) return;
			using IDXGISurface _dxgiBackBuffer = _swapChain.GetBuffer<IDXGISurface>(0);
			BitmapProperties1 bitmapProperties = new BitmapProperties1(new PixelFormat(Format.B8G8R8A8_UNorm, Vortice.DCommon.AlphaMode.Premultiplied), 96, 96, BitmapOptions.Target | BitmapOptions.CannotDraw);
			_renderTarget = _d2dDeviceContext.CreateBitmapFromDxgiSurface(_dxgiBackBuffer, bitmapProperties);
			_d2dDeviceContext.Target = _renderTarget;
		}

		private void ResizeRenderTarget()
		{
			if (_swapChain == null || _resizing) return;
			_resizing = true;
			_renderTarget?.Dispose();
			if (_d2dDeviceContext != null) _d2dDeviceContext.Target = null;
			Result res = _swapChain.ResizeBuffers(0, 0, 0, Format.Unknown, VSync ? SwapChainFlags.None : SwapChainFlags.AllowTearing);
			if (res.Failure)
			{
				throw new InvalidOperationException("Failed to resize swap chain buffers.");
			}
			CreateRenderTarget();
			_resizing = false;
		}

		private void DisposeResources()
		{
			_renderTarget?.Dispose();
			_d2dDeviceContext?.Target?.Dispose();
			_d2dDeviceContext?.Dispose();
			_swapChain?.Dispose();
			_direct2DFactory?.Dispose();
			_d3dDevice?.Dispose();
		}
	}
}
