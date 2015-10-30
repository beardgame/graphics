using System;

namespace amulware.Graphics.Meshes.ObjFile
{
    partial class ObjFileMesh
    {
        private struct Face
        {
            public readonly VertexIds[] Ids;

            public Face(VertexIds[] vertexIds)
            {
                this.Ids = vertexIds;
            }

            public struct VertexIds : IEquatable<VertexIds>
            {
                public readonly int Position;
                public readonly int UV;
                public readonly int Normal;

                public VertexIds(int position, int uv, int normal)
                {
                    this.Position = position;
                    this.UV = uv;
                    this.Normal = normal;
                }

                #region Equals and GetHashCode

                public bool Equals(VertexIds other)
                {
                    return this.Position == other.Position
                        && this.UV == other.UV
                        && this.Normal == other.Normal;
                }

                public override bool Equals(object obj)
                {
                    if (ReferenceEquals(null, obj)) return false;
                    return obj is VertexIds && this.Equals((VertexIds)obj);
                }

                public override int GetHashCode()
                {
                    unchecked
                    {
                        var hashCode = this.Position;
                        hashCode = (hashCode * 397) ^ this.UV;
                        hashCode = (hashCode * 397) ^ this.Normal;
                        return hashCode;
                    }
                }

                #endregion

            }
        }
    }
}