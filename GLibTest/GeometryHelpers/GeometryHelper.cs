using System.Numerics;
using Vortice.Direct2D1;

namespace GLibTest.GeometryHelpers
{
	class GeometryHelper
	{

		public static ID2D1PathGeometry? CreateArc(ID2D1RenderTarget renderTarget, float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			if (sweepAngle == 0f)
			{
				return null;
			}

			if (Math.Abs(sweepAngle) >= 360f)
			{
				// Create a full ellipse geometry
				ID2D1EllipseGeometry ellipse = renderTarget.Factory.CreateEllipseGeometry(new Ellipse(new Vector2(x + width / 2, y + height / 2), width / 2, height / 2));
				ID2D1PathGeometry ellipsePathGeometry = renderTarget.Factory.CreatePathGeometry();
				using (ID2D1GeometrySink ellipseSink = ellipsePathGeometry.Open())
				{
					ellipse.Outline(ellipseSink);
					ellipseSink.Close();
				}
				return ellipsePathGeometry;
			}

			// Create a partial arc
			float centerX = x + width / 2;
			float centerY = y + height / 2;
			float radiusX = width / 2;
			float radiusY = height / 2;

			float startAngleRadians = (float)(startAngle * Math.PI / 180);
			float sweepAngleRadians = (float)(sweepAngle * Math.PI / 180);

			float startX = centerX + radiusX * (float)Math.Cos(startAngleRadians);
			float startY = centerY + radiusY * (float)Math.Sin(startAngleRadians);
			float endX = centerX + radiusX * (float)Math.Cos(startAngleRadians + sweepAngleRadians);
			float endY = centerY + radiusY * (float)Math.Sin(startAngleRadians + sweepAngleRadians);

			ID2D1PathGeometry pathGeometry = renderTarget.Factory.CreatePathGeometry();
			using ID2D1GeometrySink sink = pathGeometry.Open();
			sink.BeginFigure(new Vector2(startX, startY), FigureBegin.Hollow);
			sink.AddArc(new ArcSegment
			{
				Point = new Vector2(endX, endY),
				Size = new Vortice.Mathematics.Size(radiusX, radiusY),
				SweepDirection = sweepAngle > 0 ? SweepDirection.Clockwise : SweepDirection.CounterClockwise,
				RotationAngle = 0f,
				ArcSize = Math.Abs(sweepAngle) <= 180 ? ArcSize.Small : ArcSize.Large
			});
			sink.EndFigure(FigureEnd.Open);
			sink.Close();

			return pathGeometry;
		}

		public static ID2D1PathGeometry? CreatePie(ID2D1RenderTarget renderTarget, float x, float y, float width, float height, float startAngle, float sweepAngle)
		{
			if (sweepAngle == 0f)
			{
				return null;
			}

			if (Math.Abs(sweepAngle) >= 360f)
			{
				// Create a full ellipse geometry
				ID2D1EllipseGeometry ellipse = renderTarget.Factory.CreateEllipseGeometry(new Ellipse(new Vector2(x + width / 2, y + height / 2), width / 2, height / 2));
				ID2D1PathGeometry ellipsePathGeometry = renderTarget.Factory.CreatePathGeometry();
				using (ID2D1GeometrySink ellipseSink = ellipsePathGeometry.Open())
				{
					ellipse.Outline(ellipseSink);
					ellipseSink.Close();
				}
				return ellipsePathGeometry;
			}

			// Create a partial pie
			float centerX = x + width / 2;
			float centerY = y + height / 2;
			float radiusX = width / 2;
			float radiusY = height / 2;
			float startAngleRadians = (float)(startAngle * Math.PI / 180);
			float sweepAngleRadians = (float)(sweepAngle * Math.PI / 180);
			float startX = centerX + radiusX * (float)Math.Cos(startAngleRadians);
			float startY = centerY + radiusY * (float)Math.Sin(startAngleRadians);
			float endX = centerX + radiusX * (float)Math.Cos(startAngleRadians + sweepAngleRadians);
			float endY = centerY + radiusY * (float)Math.Sin(startAngleRadians + sweepAngleRadians);
			ID2D1PathGeometry pathGeometry = renderTarget.Factory.CreatePathGeometry();
			using ID2D1GeometrySink sink = pathGeometry.Open();
			sink.BeginFigure(new Vector2(centerX, centerY), FigureBegin.Filled);
			sink.AddLine(new Vector2(startX, startY));
			sink.AddArc(new ArcSegment
			{
				Point = new Vector2(endX, endY),
				Size = new Vortice.Mathematics.Size(radiusX, radiusY),
				SweepDirection = sweepAngle > 0 ? SweepDirection.Clockwise : SweepDirection.CounterClockwise,
				RotationAngle = 0f,
				ArcSize = Math.Abs(sweepAngle) <= 180 ? ArcSize.Small : ArcSize.Large
			});
			sink.EndFigure(FigureEnd.Closed);
			sink.Close();
			return pathGeometry;
		}

		public static ID2D1PathGeometry1? CreateStar(ID2D1RenderTarget renderTarget, Vector2 center, float innerRadiusX, float innerRadiusY, float outerRadiusX, float outerRadiusY, int spikes)
		{
			// Validate the number of spikes
			if (spikes < 3) throw new ArgumentException("The number of spikes must be at least 3.", nameof(spikes));
			if (innerRadiusX <= 0 || innerRadiusY <= 0 || outerRadiusX <= 0 || outerRadiusY <= 0)
				throw new ArgumentException("All radii must be greater than 0.");

			// Create a path geometry
			using ID2D1Factory7 factory1 = renderTarget.Factory.QueryInterface<ID2D1Factory7>();
			ID2D1PathGeometry1 starGeometry = factory1.CreatePathGeometry();

			// Open the geometry for editing
			using ID2D1GeometrySink sink = starGeometry.Open();

			float angleStep = (float)(Math.PI / spikes); // Angle between each point (inner/outer)
			float currentAngle = (float)(-45 * Math.PI / 2); // Start angle pointing upwards

			// Start the figure at the first outer point
			Vector2 startPoint = new(
				center.X + (float)(outerRadiusX * Math.Cos(currentAngle)),
				center.Y + (float)(outerRadiusY * Math.Sin(currentAngle))
			);
			sink.BeginFigure(startPoint, FigureBegin.Filled);
			bool isOuter = true;

			// Loop through all points (inner and outer)
			for (int i = 0; i < (spikes * 2) - 1; i++)
			{
				isOuter = !isOuter;
				float radiusX = isOuter ? outerRadiusX : innerRadiusX;
				float radiusY = isOuter ? outerRadiusY : innerRadiusY;
				currentAngle += angleStep;

				Vector2 point = new(
					center.X + (float)(radiusX * Math.Cos(currentAngle)),
					center.Y + (float)(radiusY * Math.Sin(currentAngle))
				);

				sink.AddLine(point);
			}

			// Close the figure
			sink.EndFigure(FigureEnd.Closed);
			sink.Close();

			return starGeometry;
		}

	}
}
