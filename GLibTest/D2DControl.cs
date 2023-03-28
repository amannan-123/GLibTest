using System.Drawing;
using Vortice.DCommon;
using Vortice.Direct2D1;

namespace GLibTest
{
	public partial class D2DControl : UserControl
	{
		private ID2D1HwndRenderTarget? hwndRenderTarget;
		private ID2D1Factory? direct2DFactory;

		public D2DControl()
		{
			InitializeComponent();
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			CreateResources();
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			hwndRenderTarget?.Dispose();
			direct2DFactory?.Dispose();
		}

		private void CreateResources()
		{
			direct2DFactory = D2D1.D2D1CreateFactory<ID2D1Factory>(Vortice.Direct2D1.FactoryType.MultiThreaded);
			RenderTargetProperties renderTargetProperties = new(PixelFormat.Premultiplied)
			{
				DpiX = DeviceDpi,
				DpiY = DeviceDpi,
				Type = RenderTargetType.Hardware,
				Usage = RenderTargetUsage.None,
			};
			HwndRenderTargetProperties hwndRenderTargetProperties = new()
			{
				Hwnd = Handle,
				PixelSize = Size,
				PresentOptions = PresentOptions.None
			};
			hwndRenderTarget = direct2DFactory.CreateHwndRenderTarget(renderTargetProperties, hwndRenderTargetProperties);
		}


		public delegate void Render(Object sender, ID2D1HwndRenderTarget g);  // delegate
		public event Render? OnRendering;

		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (hwndRenderTarget == null)
				return;

			hwndRenderTarget?.BeginDraw();
			OnRendering?.Invoke(this, hwndRenderTarget!);
			hwndRenderTarget?.EndDraw();
		}

		protected virtual void OnRender(ID2D1RenderTarget g) { }

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			hwndRenderTarget?.Resize(Size);
			Invalidate();
		}

	}
}
