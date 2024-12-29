using System.Numerics;

namespace GLibTest.PathHelpers
{
	class D2dPathNode(Vector2 point, D2dPathNode.NodeType type)
	{
		public enum NodeType
		{
			Vertex,
			Control
		}

		public Vector2 Point { get; set; } = point;
		public NodeType Type { get; set; } = type;
	}
}