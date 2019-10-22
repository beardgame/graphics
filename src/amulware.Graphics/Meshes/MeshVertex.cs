using System.Runtime.InteropServices;
using OpenTK;

namespace amulware.Graphics.Meshes
{
    /// <summary>
    /// A basic vertex for mesh for mesh rendering. Consists of a position and a normal.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MeshVertex : IVertexData
    {
        private readonly Vector3 position;
        private readonly Vector3 normal;

        /// <summary>
        /// Creates a new mesh vertex with a given position.
        /// </summary>
        public MeshVertex(Vector3 position)
            : this(position, new Vector3(0))
        {
        }

        /// <summary>
        /// Creates a new mesh vertex with a given position and normal.
        /// </summary>
        public MeshVertex(Vector3 position, Vector3 normal)
        {
            this.position = position;
            this.normal = normal;
        }

        /// <summary>
        /// The position of the vertex.
        /// </summary>
        public Vector3 Position { get { return this.position; } }
        /// <summary>
        /// The normal of the vertex.
        /// </summary>
        public Vector3 Normal {get { return this.normal; } }

        #region IVertexData

        private static VertexAttribute[] vertexArray;

        public VertexAttribute[] VertexAttributes()
        {
            return vertexArray ?? (vertexArray = makeVertexArray());
        }

        private static VertexAttribute[] makeVertexArray()
        {
            return VertexData.MakeAttributeArray(
                VertexData.MakeAttributeTemplate<Vector3>("v_position"),
                VertexData.MakeAttributeTemplate<Vector3>("v_normal")
                );
        }

        #endregion
    }
}
