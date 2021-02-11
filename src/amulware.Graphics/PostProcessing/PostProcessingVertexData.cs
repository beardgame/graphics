using System.Runtime.InteropServices;
using amulware.Graphics.Vertices;
using OpenTK.Mathematics;
using static amulware.Graphics.Vertices.VertexData;

namespace amulware.Graphics.PostProcessing
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct PostProcessingVertexData : IVertexData
    {
        private readonly Vector2 position;

        public VertexAttribute[] VertexAttributes => vertexAttributes;

        private static VertexAttribute[] vertexAttributes { get; } = MakeAttributeArray(
            MakeAttributeTemplate<Vector2>("v_position")
        );

        public PostProcessingVertexData(Vector2 position)
        {
            this.position = position;
        }

        public override string ToString() => $"{position}";
    }
}
