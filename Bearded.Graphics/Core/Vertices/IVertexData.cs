using System.Collections.Immutable;

namespace Bearded.Graphics.Vertices;

public interface IVertexData
{
    static abstract ImmutableArray<VertexAttribute> VertexAttributes { get; }
}
