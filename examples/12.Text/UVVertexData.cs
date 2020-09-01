using System.Runtime.InteropServices;
using amulware.Graphics.Vertices;
using OpenToolkit.Mathematics;
using static amulware.Graphics.Vertices.VertexData;

namespace amulware.Graphics.Examples.Text
{
    [StructLayout(LayoutKind.Sequential)]
    readonly struct UVVertexData : IVertexData
    {
        private readonly Vector3 position;
        private readonly Vector2 uv;

        public VertexAttribute[] VertexAttributes => vertexAttributes;

        private static VertexAttribute[] vertexAttributes { get; } = MakeAttributeArray(
            MakeAttributeTemplate<Vector3>("v_position"),
            MakeAttributeTemplate<Vector2>("v_uv")
        );

        public UVVertexData(Vector3 position, Vector2 uv)
        {
            this.position = position;
            this.uv = uv;
        }

        public override string ToString() => $"{position}, {uv}";
    }
}
