
namespace Bearded.Graphics.Vertices
{
    /// <summary>
    /// This interface must be implemented by any custom vertex data.
    /// </summary>
    public interface IVertexData
    {
        /// <summary>
        /// Returns the vertex' <see cref="VertexAttributes"/>
        /// </summary>
        /// <returns>Array of <see cref="VertexAttribute"/></returns>
        VertexAttribute[] VertexAttributes { get; }
    }
}
