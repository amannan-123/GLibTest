using SharpGen.Runtime;
using System.Numerics;
using Vortice.Direct2D1;

namespace GLibTest.PathHelpers
{
	public class CustomGeometrySink(ID2D1RenderTarget renderTarget) : CallbackBase, ID2D1SimplifiedGeometrySink
	{
		private readonly ID2D1RenderTarget renderTarget = renderTarget;

		private D2DGraphicsPath Path { get; set; } = new();

		public void BeginFigure(Vector2 startPoint, FigureBegin figureBegin)
		{
			Path.AddNode(startPoint, D2dPathNode.NodeType.Vertex);
		}

		public void EndFigure(FigureEnd figureEnd)
		{
			Path.Closed = figureEnd == FigureEnd.Closed;
		}

		public void Close() { }

		public void SetFillMode(FillMode fillMode) { }

		public void SetSegmentFlags(PathSegment vertexFlags) { }

		public void AddLines(Vector2[] points)
		{
			foreach (var point in points)
			{
				Path.AddNode(point, D2dPathNode.NodeType.Vertex);
			}
		}

		public void AddBeziers(BezierSegment[] beziers)
		{
			foreach (var bezier in beziers)
			{
				Path.AddNode(bezier.Point1, D2dPathNode.NodeType.Control);
				Path.AddNode(bezier.Point2, D2dPathNode.NodeType.Control);
				Path.AddNode(bezier.Point3, D2dPathNode.NodeType.Vertex);
			}
		}

		public void Draw()
		{
			Path.Draw(renderTarget);
		}

	}
}
