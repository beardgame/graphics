using System;
using System.Collections.Generic;
using System.Linq;
using OpenToolkit.Mathematics;

namespace amulware.Graphics.Meshes.ObjFile
{
    public delegate TVertex ObjFileVertexParser<out TVertex>(
        Vector4 position, Vector3 uvCoordinates, Vector3 normal)
        where TVertex : struct;

    partial class ObjFileMesh
    {
        private bool calculatedStats;
        private int triangleCount;
        private int totalFaceVertexCount;

        /// <summary>
        /// Creates a mesh from the loaded obj file for further processing and rendering.
        /// </summary>
        /// <param name="mergeVertices">Whether to merge and reuse identical vertices.</param>
        /// <param name="normalizeNormals">Whether to normalise normals before creating the mesh vertex.</param>
        public Mesh<MeshVertex> ToMesh(
            bool mergeVertices = true,
            bool normalizeNormals = true)
        {
            return this.ToMesh(
                meshVertexParser,
                mergeVertices,
                normalizeNormals
                );
        }

        /// <summary>
        /// Creates a mesh from the loaded obj file for further processing and rendering.
        /// </summary>
        /// <param name="vertexParser">A function creating mesh vertices from position, uv coordinate and normal.</param>
        /// <param name="mergeVertices">Whether to merge and reuse identical vertices.</param>
        /// <param name="normalizeNormals">Whether to normalise normals before creating the mesh vertex.</param>
        public Mesh<TVertex> ToMesh<TVertex>(
            ObjFileVertexParser<TVertex> vertexParser,
            bool mergeVertices = true,
            bool normalizeNormals = true)
            where TVertex : struct
        {
            this.ensureStats();

            if (normalizeNormals)
            {
                vertexParser = normalizeNormal(vertexParser);
            }

            var builder = new Mesh<TVertex>.Builder(
                mergeVertices ? 0 : this.totalFaceVertexCount);

            var vertexAdder = mergeVertices
                ? new SimpleVertexAdder<TVertex>(builder, vertexParser, this)
                : new MergedVertexDictionary<TVertex>(builder, vertexParser, this);

            return this.toMesh(builder, vertexAdder);
        }

        private static MeshVertex meshVertexParser(Vector4 position, Vector3 uv, Vector3 normal)
        {
            return new MeshVertex(position.Xyz, normal);
        }

        private static ObjFileVertexParser<TVertex> normalizeNormal<TVertex>(ObjFileVertexParser<TVertex> parser)
            where TVertex : struct
        {
            return (p, uv, n) =>
                {
                    var normalLength = n.LengthSquared;

                    if (normalLength > 0)
                    {
                        n = n / (float)Math.Sqrt(normalLength);
                    }

                    return parser(p, uv, n);
                };
        }

        private void ensureStats()
        {
            if (this.calculatedStats)
                return;

            this.triangleCount = this.faces
                .Sum(f => f.Ids.Length - 2);

            this.totalFaceVertexCount =
                this.triangleCount + this.faces.Count * 2;

            this.calculatedStats = true;
        }

        private Mesh<TVertex> toMesh<TVertex>(Mesh<TVertex>.Builder builder, IVertexAdder vertexAdder)
            where TVertex : struct
        {
            foreach (var face in this.faces)
            {
                addFaceToMeshBuilder(face, vertexAdder, builder);
            }

            return builder.Build();
        }

        private static void addFaceToMeshBuilder<TVertex>(Face face,
            IVertexAdder mergedVertices, Mesh<TVertex>.Builder builder)
            where TVertex : struct
        {
            var vId0 = mergedVertices[face.Ids[0]];
            var vId1 = mergedVertices[face.Ids[1]];

            for (int i = 2; i < face.Ids.Length; i++)
            {
                var vId2 = mergedVertices[face.Ids[i]];

                builder.AddTriangle(
                    new IndexTriangle(vId0, vId1, vId2)
                    );

                vId1 = vId2;
            }
        }

        private TVertex createVertex<TVertex>(
            Face.VertexIds ids, ObjFileVertexParser<TVertex> parser)
            where TVertex : struct
        {
            var position = this.positions[ids.Position];
            var uv = ids.UV < 2
                ? new Vector3(0)
                : this.uvCoordinates[ids.UV];
            var normal = ids.UV < 0
                ? new Vector3(0)
                : this.normals[ids.Normal];

            return parser(position, uv, normal);
        }

        #region IVertexAdder and implementations

        private interface IVertexAdder
        {
            int this[Face.VertexIds vIds] { get; }
        }

        private class SimpleVertexAdder<TVertex> : IVertexAdder
            where TVertex : struct
        {
            private readonly Mesh<TVertex>.Builder builder;
            private readonly ObjFileVertexParser<TVertex> parser;
            private readonly ObjFileMesh owner;

            public SimpleVertexAdder(Mesh<TVertex>.Builder builder,
                ObjFileVertexParser<TVertex> parser, ObjFileMesh owner)
            {
                this.builder = builder;
                this.parser = parser;
                this.owner = owner;
            }

            public virtual int this[Face.VertexIds vIds]
            {
                get
                {
                    return this.builder.AddVertices(
                        this.owner.createVertex(vIds, this.parser)
                        );
                }
            }
        }

        private class MergedVertexDictionary<TVertex> : SimpleVertexAdder<TVertex>
            where TVertex : struct
        {
            private readonly Dictionary<Face.VertexIds, int> vertices =
                new Dictionary<Face.VertexIds, int>();

            public MergedVertexDictionary(Mesh<TVertex>.Builder builder,
                ObjFileVertexParser<TVertex> parser, ObjFileMesh owner)
                : base(builder, parser, owner)
            {
            }

            public override int this[Face.VertexIds vIds]
            {
                get
                {
                    int id;
                    if (!this.vertices.TryGetValue(vIds, out id))
                    {
                        id = base[vIds];
                        this.vertices.Add(vIds, id);
                    }
                    return id;
                }
            }
        }

        #endregion
    }
}
