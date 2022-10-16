using System;
using System.Runtime.CompilerServices;
using Bearded.Graphics.MeshBuilders;
using Bearded.Graphics.Vertices;
using Bearded.Utilities;
using OpenTK.Mathematics;

namespace Bearded.Graphics.Shapes
{
    public sealed class ShapeDrawer3<TVertex, TVertexParameters> : IShapeDrawer3<TVertexParameters> where TVertex : struct, IVertexData
    {
        public delegate TVertex CreateShapeVertex(Vector3 xyz, TVertexParameters parameters);

        private readonly IIndexedTrianglesMeshBuilder<TVertex, ushort> meshBuilder;
        private readonly CreateShapeVertex createVertex;

        public ShapeDrawer3(
            IIndexedTrianglesMeshBuilder<TVertex, ushort> meshBuilder, CreateShapeVertex createVertex)
        {
            this.meshBuilder = meshBuilder;
            this.createVertex = createVertex;
        }

        public void DrawTetrahedron(Vector3 center, float scale, TVertexParameters parameters)
        {
            const float sqrt2 = 1.41421356237f;
            const float sqrt6 = 2.44948974278f;

            meshBuilder.Add(4, 12, out var vertices, out var indices, out var indexOffset);

            vertices[0] = createVertex(center + new Vector3(0, 0, scale), parameters);
            vertices[1] = createVertex(center + new Vector3(sqrt2 * 2f / 3f, 0, -1f / 3f) * scale, parameters);
            vertices[2] = createVertex(center + new Vector3(-sqrt2 / 3f, sqrt6 / 3f, -1f / 3f) * scale, parameters);
            vertices[3] = createVertex(center + new Vector3(-sqrt2 / 3f, -sqrt6 / 3f, -1f / 3f) * scale, parameters);

            Span<int> localIndices = stackalloc int[12]
            {
                0, 1, 2,
                0, 2, 3,
                0, 3, 1,
                1, 3, 2,
            };

            copyIndices(localIndices, indices, indexOffset);
        }

        public void DrawOctahedron(Vector3 center, float scale, TVertexParameters parameters)
        {
            meshBuilder.Add(6, 24, out var vertices, out var indices, out var indexOffset);

            vertices[0] = createVertex(center + new Vector3(scale, 0, 0), parameters);
            vertices[1] = createVertex(center + new Vector3(-scale, 0, 0), parameters);
            vertices[2] = createVertex(center + new Vector3(0, scale, 0), parameters);
            vertices[3] = createVertex(center + new Vector3(0, -scale, 0), parameters);
            vertices[4] = createVertex(center + new Vector3(0, 0, scale), parameters);
            vertices[5] = createVertex(center + new Vector3(0, 0, -scale), parameters);

            Span<int> localIndices = stackalloc int[24]
            {
                4, 0, 2,
                4, 2, 1,
                4, 1, 3,
                4, 3, 0,
                5, 2, 0,
                5, 1, 2,
                5, 3, 1,
                5, 0, 3,
            };

            copyIndices(localIndices, indices, indexOffset);
        }

        public void DrawCuboid(float x, float y, float z, float w, float h, float d, TVertexParameters parameters)
        {
            meshBuilder.Add(8, 36, out var vertices, out var indices, out var indexOffset);

            vertices[0] = createVertex(new Vector3(x, y, z), parameters);
            vertices[1] = createVertex(new Vector3(x + w, y, z), parameters);
            vertices[2] = createVertex(new Vector3(x + w, y + h, z), parameters);
            vertices[3] = createVertex(new Vector3(x, y + h, z), parameters);

            vertices[4] = createVertex(new Vector3(x, y, z + d), parameters);
            vertices[5] = createVertex(new Vector3(x + w, y, z + d), parameters);
            vertices[6] = createVertex(new Vector3(x + w, y + h, z + d), parameters);
            vertices[7] = createVertex(new Vector3(x, y + h, z + d), parameters);

            Span<int> localIndices = stackalloc int[36]
            {
                0, 3, 2,
                0, 2, 1,
                0, 1, 5,
                0, 5, 4,
                0, 4, 7,
                0, 7, 3,
                6, 5, 1,
                6, 1, 2,
                6, 2, 3,
                6, 3, 7,
                6, 7, 4,
                6, 4, 5,
            };

            copyIndices(localIndices, indices, indexOffset);
        }

        public void DrawDodecahedron(Vector3 center, float scale, TVertexParameters parameters)
        {
            var a = 0.57735026919f * scale; // 1/sqrt(3)
            var b = 0.35682208977f * scale; // sqrt((3-sqrt(5)/6)
            var c = 0.93417235896f * scale; // sqrt((3+sqrt(5)/6)

            meshBuilder.Add(20, 108, out var vertices, out var indices, out var indexOffset);

            vertices[0] = createVertex(center + new Vector3(a, a, a), parameters);
            vertices[1] = createVertex(center + new Vector3(a, a, -a), parameters);
            vertices[2] = createVertex(center + new Vector3(a, -a, a), parameters);
            vertices[3] = createVertex(center + new Vector3(a, -a, -a), parameters);
            vertices[4] = createVertex(center + new Vector3(-a, a, a), parameters);
            vertices[5] = createVertex(center + new Vector3(-a, a, -a), parameters);
            vertices[6] = createVertex(center + new Vector3(-a, -a, a), parameters);
            vertices[7] = createVertex(center + new Vector3(-a, -a, -a), parameters);
            vertices[8] = createVertex(center + new Vector3(b, c, 0), parameters);
            vertices[9] = createVertex(center + new Vector3(-b, c, 0), parameters);
            vertices[10] = createVertex(center + new Vector3(b, -c, 0), parameters);
            vertices[11] = createVertex(center + new Vector3(-b, -c, 0), parameters);
            vertices[12] = createVertex(center + new Vector3(c, 0, b), parameters);
            vertices[13] = createVertex(center + new Vector3(c, 0, -b), parameters);
            vertices[14] = createVertex(center + new Vector3(-c, 0, b), parameters);
            vertices[15] = createVertex(center + new Vector3(-c, 0, -b), parameters);
            vertices[16] = createVertex(center + new Vector3(0, b, c), parameters);
            vertices[17] = createVertex(center + new Vector3(0, -b, c), parameters);
            vertices[18] = createVertex(center + new Vector3(0, b, -c), parameters);
            vertices[19] = createVertex(center + new Vector3(0, -b, -c), parameters);

            Span<int> localIndices = stackalloc int[108]
            {
                0, 8, 9,
                0, 9, 4,
                0, 4, 16,
                0, 12, 13,
                0, 13, 1,
                0, 1, 8,
                0, 16, 17,
                0, 17, 2,
                0, 2, 12,
                8, 1, 18,
                8, 18, 5,
                8, 5, 9,
                12, 2, 10,
                12, 10, 3,
                12, 3, 13,
                16, 4, 14,
                16, 14, 6,
                16, 6, 17,
                9, 5, 15,
                9, 15, 14,
                9, 14, 4,
                6, 11, 10,
                6, 10, 2,
                6, 2, 17,
                3, 19, 18,
                3, 18, 1,
                3, 1, 13,
                7, 15, 5,
                7, 5, 18,
                7, 18, 19,
                7, 11, 6,
                7, 6, 14,
                7, 14, 15,
                7, 19, 3,
                7, 3, 10,
                7, 10, 11,
            };

            copyIndices(localIndices, indices, indexOffset);
        }

        public void DrawIcosahedron(Vector3 center, float scale, TVertexParameters parameters)
        {
            const float t = 1.61803398875f; // (1+sqrt(5))/2
            const float s = 1.90211303259f; // sqrt(1+t^2)

            var a = scale * t / s;
            var b = scale / s;

            meshBuilder.Add(12, 60, out var vertices, out var indices, out var indexOffset);

            vertices[0] = createVertex(center + new Vector3(a, b, 0), parameters);
            vertices[1] = createVertex(center + new Vector3(-a, b, 0), parameters);
            vertices[2] = createVertex(center + new Vector3(a, -b, 0), parameters);
            vertices[3] = createVertex(center + new Vector3(-a, -b, 0), parameters);
            vertices[4] = createVertex(center + new Vector3(b, 0, a), parameters);
            vertices[5] = createVertex(center + new Vector3(b, 0, -a), parameters);
            vertices[6] = createVertex(center + new Vector3(-b, 0, a), parameters);
            vertices[7] = createVertex(center + new Vector3(-b, 0, -a), parameters);
            vertices[8] = createVertex(center + new Vector3(0, a, b), parameters);
            vertices[9] = createVertex(center + new Vector3(0, -a, b), parameters);
            vertices[10] = createVertex(center + new Vector3(0, a, -b), parameters);
            vertices[11] = createVertex(center + new Vector3(0, -a, -b), parameters);

            Span<int> localIndices = stackalloc int[60]
            {
                0, 8, 4,
                0, 5, 10,
                2, 4, 9,
                2, 11, 5,
                1, 6, 8,
                1, 10, 7,
                3, 9, 6,
                3, 7, 11,
                0, 10, 8,
                1, 8, 10,
                2, 9, 11,
                3, 11, 9,
                4, 2, 0,
                5, 0, 2,
                6, 1, 3,
                7, 3, 1,
                8, 6, 4,
                9, 4, 6,
                10, 5, 7,
                11, 7, 5,
            };

            copyIndices(localIndices, indices, indexOffset);
        }

        public void DrawCone(
            Vector3 baseCenter, Vector3 baseToApex, float baseRadius, TVertexParameters parameters, int edges = 32)
        {
            if (edges < 3)
                throw new ArgumentException("Cone must have at least three edges");

            var baseToApexNormalized = baseToApex.NormalizedSafe();

            var stepAngle = MathHelper.TwoPi / edges;
            var stepRotation = Matrix3.CreateFromAxisAngle(baseToApex, stepAngle);

            var unitX = baseToApexNormalized == Vector3.UnitZ
                ? Vector3.UnitX
                : Vector3.Cross(baseToApexNormalized, Vector3.UnitZ);

            var vertexCount = edges + 1;
            var indexCount = (edges + edges - 2) * 3;

            var baseVertexOffset = unitX * baseRadius;

            meshBuilder.Add(vertexCount, indexCount, out var vertices, out var indices, out var indexOffset);

            for (var i = 0; i < edges; i++)
            {
                vertices[i] = createVertex(baseCenter + baseVertexOffset, parameters);
                baseVertexOffset *= stepRotation;

                indices[i * 3] = (ushort)(indexOffset + edges);
                indices[i * 3 + 1] = (ushort)(indexOffset + i);
                indices[i * 3 + 2] = (ushort)(indexOffset + (i + 1) % edges);
            }

            vertices[edges] = createVertex(baseCenter + baseToApex, parameters);

            var o = edges * 3;
            for (var i = 0; i < edges - 2; i++)
            {
                indices[o] = indexOffset;
                indices[o + 1] = (ushort)(indexOffset + i + 2);
                indices[o + 2] = (ushort)(indexOffset + i + 1);
                o += 3;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void copyIndices(Span<int> localIndices, Span<ushort> indices, int indexOffset)
        {
            for (var i = 0; i < indices.Length; i++)
            {
                indices[i] = (ushort) (indexOffset + localIndices[i]);
            }
        }
    }
}
