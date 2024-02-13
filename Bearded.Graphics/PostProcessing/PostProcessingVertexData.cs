using System.Collections.Immutable;
using System.Runtime.InteropServices;
using Bearded.Graphics.Vertices;
using OpenTK.Mathematics;
using static Bearded.Graphics.Vertices.VertexData;

namespace Bearded.Graphics.PostProcessing;

[StructLayout(LayoutKind.Sequential)]
public readonly struct PostProcessingVertexData : IVertexData
{
    private readonly Vector2 position;

    public static ImmutableArray<VertexAttribute> VertexAttributes { get; }
        = MakeAttributeArray(
            MakeAttributeTemplate<Vector2>("v_position")
        );

    public PostProcessingVertexData(Vector2 position)
    {
        this.position = position;
    }

    public override string ToString() => $"{position}";
}
