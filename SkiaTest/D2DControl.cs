using unvell.D2DLib;
using unvell.D2DLib.WinForm;

namespace SkiaTest
{
	public class D2DControl : UserControl
	{
		public D2DControl()
		{
		
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
		}

		private D2DDevice? device;

		public D2DDevice Device
		{
			get
			{
				var hwnd = this.Handle;
				this.device ??= D2DDevice.FromHwnd(hwnd);
				return this.device;
			}
		}

		private D2DGraphics? graphics;

		protected override void CreateHandle()
		{
			base.CreateHandle();

			this.DoubleBuffered = false;

			this.device ??= D2DDevice.FromHwnd(this.Handle);

			this.graphics = new D2DGraphics(this.device);

		}

		public delegate void Render(Object sender, D2DGraphics g);  // delegate
		public event Render? OnRendering;

		protected override void OnPaintBackground(PaintEventArgs e) { }

		protected override void OnPaint(PaintEventArgs e)
		{
			if (this.graphics == null) return;

			this.graphics.BeginRender();
			OnRendering?.Invoke(this, this.graphics);
			this.graphics.EndRender();
		}

		protected override void DestroyHandle()
		{
			base.DestroyHandle();
			this.device?.Dispose();
		}

		protected virtual void OnRender(D2DGraphics g) { }

		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case (int)Win32.WMessages.WM_ERASEBKGND:
					break;

				case (int)Win32.WMessages.WM_SIZE:
					base.WndProc(ref m);
					this.device?.Resize();
					break;

				default:
					base.WndProc(ref m);
					break;
			}
		}

		public new void Invalidate()
		{
			base.Invalidate(false);
		}
	}
}
