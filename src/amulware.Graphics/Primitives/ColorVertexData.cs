using System.Runtime.InteropServices;
using amulware.Graphics.Vertices;
using OpenToolkit.Mathematics;
using static amulware.Graphics.Vertices.VertexData;

namespace amulware.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ColorVertexData : IVertexData
    {
        private readonly Vector3 position;

        private readonly Color color;

        public VertexAttribute[] VertexAttributes => vertexAttributes;

        private static VertexAttribute[] vertexAttributes { get; } = MakeAttributeArray(
            MakeAttributeTemplate<Vector3>("v_position"),
            MakeAttributeTemplate<Color>("v_color")
        );

        public ColorVertexData(float x, float y, float z, Color color) : this(new Vector3(x, y, z), color) {}

        public ColorVertexData(Vector3 position, Color color)
        {
            this.position = position;
            this.color = color;
        }

        public override string ToString() => $"{position}, {color}";
    }
}
