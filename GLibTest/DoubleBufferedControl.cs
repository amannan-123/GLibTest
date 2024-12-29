namespace GLibTest
{
	public partial class DoubleBufferedControl : UserControl
	{
		public DoubleBufferedControl()
		{
			InitializeComponent();
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
		}
	}
}
