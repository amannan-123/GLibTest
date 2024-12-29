using System.Numerics;
using Vortice.Direct2D1;
using Vortice.DirectWrite;
using Vortice.Mathematics;

namespace GLibTest.PathHelpers
{
	class D2DGraphicsPath
	{
		public List<D2dPathNode> Nodes { get; set; } = [];
		public bool Closed { get; set; }

		private const float rX = 10;
		private const float rY = 10;
		private const float fontSize = 10;

		public void AddNode(Vector2 point, D2dPathNode.NodeType type)
		{
			Nodes.Add(new D2dPathNode(point, type));
		}

		public void Draw(ID2D1RenderTarget renderTarget)
		{
			if (Nodes.Count == 0) return;
			if (Nodes[0].Type != D2dPathNode.NodeType.Vertex) return;

			using IDWriteFactory dwriteFactory = DWrite.DWriteCreateFactory<IDWriteFactory>(Vortice.DirectWrite.FactoryType.Shared);
			using IDWriteTextFormat textFormat = dwriteFactory.CreateTextFormat(
					"Arial", null, FontWeight.Bold, Vortice.DirectWrite.FontStyle.Normal, FontStretch.Normal, fontSize, "en-us");
			textFormat.TextAlignment = TextAlignment.Center;
			textFormat.ParagraphAlignment = ParagraphAlignment.Center;

			using ID2D1SolidColorBrush vertexBrush = renderTarget.CreateSolidColorBrush(new Color4(1, 1, 0));
			using ID2D1SolidColorBrush controlBrush = renderTarget.CreateSolidColorBrush(new Color4(0, 1, 0));
			using ID2D1SolidColorBrush lineBrush = renderTarget.CreateSolidColorBrush(new Color4(0, 0, 1));
			using ID2D1SolidColorBrush textBrush = renderTarget.CreateSolidColorBrush(new Color4(0, 0, 0));
			using ID2D1SolidColorBrush pathBrush = renderTarget.CreateSolidColorBrush(new Color4(1, 1, 1));

			ID2D1StrokeStyle geometryStrokeStyle = renderTarget.Factory.CreateStrokeStyle(new StrokeStyleProperties
			{
				LineJoin = LineJoin.Round,
				StartCap = CapStyle.Round,
				EndCap = CapStyle.Round,
				DashStyle = DashStyle.DashDotDot,
			});

			// Create and populate the path geometry
			using ID2D1PathGeometry pathGeometry = renderTarget.Factory.CreatePathGeometry();
			using ID2D1GeometrySink sink = pathGeometry.Open();

			sink.BeginFigure(Nodes[0].Point, FigureBegin.Filled);

			for (int i = 1; i < Nodes.Count; i++)
			{
				D2dPathNode node = Nodes[i];
				if (node.Type == D2dPathNode.NodeType.Vertex)
				{
					sink.AddLine(node.Point);
				}
				else if (Nodes[i].Type == D2dPathNode.NodeType.Control)
				{
					BezierSegment bezier = new()
					{
						Point1 = node.Point,
						Point2 = Nodes[i + 1].Point,
						Point3 = Nodes[i + 2].Point
					};
					sink.AddBezier(bezier);
					i += 2;
				}
			}
			sink.EndFigure(Closed ? FigureEnd.Closed : FigureEnd.Open);
			sink.Close();

			// Draw the path
			renderTarget.DrawGeometry(pathGeometry, pathBrush, 2, geometryStrokeStyle);

			// Draw nodes and lines
			foreach (var node in Nodes)
			{
				if (node.Type == D2dPathNode.NodeType.Vertex)
				{
					int index = Nodes.IndexOf(node);
					if (index < Nodes.Count - 1 && Nodes[index + 1].Type == D2dPathNode.NodeType.Control)
					{
						renderTarget.DrawLine(Nodes[index + 1].Point, node.Point, lineBrush, 2);
					}
				}
				else if (node.Type == D2dPathNode.NodeType.Control)
				{
					int index = Nodes.IndexOf(node);
					if (index < Nodes.Count - 1 && Nodes[index + 1].Type == D2dPathNode.NodeType.Vertex)
					{
						renderTarget.DrawLine(Nodes[index + 1].Point, node.Point, lineBrush, 2);
					}
				}

				renderTarget.FillEllipse(new Ellipse(node.Point, rX, rY), node.Type == D2dPathNode.NodeType.Vertex ? vertexBrush : controlBrush);
				Rect textRect = new(node.Point.X, node.Point.Y, 0, 0);
				textRect.Inflate(rX, rY);
				renderTarget.DrawText(node.Type == D2dPathNode.NodeType.Vertex ? "V" : "C", textFormat, textRect, textBrush);
			}
		}

	}
}
