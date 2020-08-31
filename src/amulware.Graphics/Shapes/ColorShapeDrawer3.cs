using System;
using System.Runtime.CompilerServices;
using amulware.Graphics.MeshBuilders;
using OpenToolkit.Mathematics;

namespace amulware.Graphics.Shapes
{
    public sealed class ColorShapeDrawer3
    {
        // TODO: we are assuming this will render as triangles - bad naming? do we need to change the abstraction?
        private readonly IIndexedMeshBuilder<ColorVertexData, ushort> meshBuilder;

        public ColorShapeDrawer3(IIndexedMeshBuilder<ColorVertexData, ushort> meshBuilder)
        {
            this.meshBuilder = meshBuilder;
        }

        public void DrawTetrahedron(Vector3 center, float scale, Color color)
        {
            const float sqrt2 = 1.41421356237f;
            const float sqrt6 = 2.44948974278f;

            meshBuilder.Add(4, 12, out var vertices, out var indices, out var indexOffset);

            vertices[0] = new ColorVertexData(center + new Vector3(0, 0, scale), color);
            vertices[1] = new ColorVertexData(center + new Vector3(sqrt2 * 2f / 3f, 0, -1f / 3f) * scale, color);
            vertices[2] = new ColorVertexData(center + new Vector3(-sqrt2 / 3f, sqrt6 / 3f, -1f / 3f) * scale, color);
            vertices[3] = new ColorVertexData(center + new Vector3(-sqrt2 / 3f, -sqrt6 / 3f, -1f / 3f) * scale, color);

            Span<int> localIndices = stackalloc int[12]
            {
                0, 1, 2,
                0, 2, 3,
                0, 3, 1,
                1, 3, 2
            };

            copyIndices(localIndices, indices, indexOffset);
        }

        public void DrawOctahedron(Vector3 center, float scale, Color color)
        {
            meshBuilder.Add(6, 24, out var vertices, out var indices, out var indexOffset);

            vertices[0] = new ColorVertexData(center + new Vector3(scale, 0, 0), color);
            vertices[1] = new ColorVertexData(center + new Vector3(-scale, 0, 0), color);
            vertices[2] = new ColorVertexData(center + new Vector3(0, scale, 0), color);
            vertices[3] = new ColorVertexData(center + new Vector3(0, -scale, 0), color);
            vertices[4] = new ColorVertexData(center + new Vector3(0, 0, scale), color);
            vertices[5] = new ColorVertexData(center + new Vector3(0, 0, -scale), color);

            Span<int> localIndices = stackalloc int[24]
            {
                4, 0, 2,
                4, 2, 1,
                4, 1, 3,
                4, 3, 0,
                5, 2, 0,
                5, 1, 2,
                5, 3, 1,
                5, 0, 3
            };

            copyIndices(localIndices, indices, indexOffset);
        }

        public void DrawCube(Vector3 center, float scale, Color color)
        {
            const float u = 0.5f;

            var (x, y, z) = center;
            DrawCuboid(x - u * scale, y - u * scale, z - u * scale, scale, scale, scale, color);
        }

        public void DrawCuboid(Vector3 xyz, Vector3 whd, Color color)
        {
            var (x, y, z) = xyz;
            var (w, h, d) = whd;
            DrawCuboid(x, y, z, w, h, d, color);
        }

        public void DrawCuboid(float x, float y, float z, float w, float h, float d, Color color)
        {
            meshBuilder.Add(8, 36, out var vertices, out var indices, out var indexOffset);

            vertices[0] = new ColorVertexData(x, y, z, color);
            vertices[1] = new ColorVertexData(x + w, y, z, color);
            vertices[2] = new ColorVertexData(x + w, y + h, z, color);
            vertices[3] = new ColorVertexData(x, y + h, z, color);

            vertices[4] = new ColorVertexData(x, y, z + d, color);
            vertices[5] = new ColorVertexData(x + w, y, z + d, color);
            vertices[6] = new ColorVertexData(x + w, y + h, z + d, color);
            vertices[7] = new ColorVertexData(x, y + h, z + d, color);

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
                6, 4, 5
            };

            copyIndices(localIndices, indices, indexOffset);
        }

        public void DrawDodecahedron(Vector3 center, float scale, Color color)
        {
            var a = 0.57735026919f * scale; // 1/sqrt(3)
            var b = 0.35682208977f * scale; // sqrt((3-sqrt(5)/6)
            var c = 0.93417235896f * scale; // sqrt((3+sqrt(5)/6)

            meshBuilder.Add(20, 108, out var vertices, out var indices, out var indexOffset);

            vertices[0] = new ColorVertexData(center + new Vector3(a, a, a), color);
            vertices[1] = new ColorVertexData(center + new Vector3(a, a, -a), color);
            vertices[2] = new ColorVertexData(center + new Vector3(a, -a, a), color);
            vertices[3] = new ColorVertexData(center + new Vector3(a, -a, -a), color);
            vertices[4] = new ColorVertexData(center + new Vector3(-a, a, a), color);
            vertices[5] = new ColorVertexData(center + new Vector3(-a, a, -a), color);
            vertices[6] = new ColorVertexData(center + new Vector3(-a, -a, a), color);
            vertices[7] = new ColorVertexData(center + new Vector3(-a, -a, -a), color);
            vertices[8] = new ColorVertexData(center + new Vector3(b, c, 0), color);
            vertices[9] = new ColorVertexData(center + new Vector3(-b, c, 0), color);
            vertices[10] = new ColorVertexData(center + new Vector3(b, -c, 0), color);
            vertices[11] = new ColorVertexData(center + new Vector3(-b, -c, 0), color);
            vertices[12] = new ColorVertexData(center + new Vector3(c, 0, b), color);
            vertices[13] = new ColorVertexData(center + new Vector3(c, 0, -b), color);
            vertices[14] = new ColorVertexData(center + new Vector3(-c, 0, b), color);
            vertices[15] = new ColorVertexData(center + new Vector3(-c, 0, -b), color);
            vertices[16] = new ColorVertexData(center + new Vector3(0, b, c), color);
            vertices[17] = new ColorVertexData(center + new Vector3(0, -b, c), color);
            vertices[18] = new ColorVertexData(center + new Vector3(0, b, -c), color);
            vertices[19] = new ColorVertexData(center + new Vector3(0, -b, -c), color);

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
                7, 10, 11
            };

            copyIndices(localIndices, indices, indexOffset);
        }

        public void DrawIcosahedron(Vector3 center, float scale, Color color)
        {
            const float t = 1.61803398875f; // (1+sqrt(5))/2
            const float s = 1.90211303259f; // sqrt(1+t^2)

            var a = scale * t / s;
            var b = scale / s;

            meshBuilder.Add(12, 60, out var vertices, out var indices, out var indexOffset);

            vertices[0] = new ColorVertexData(center + new Vector3(a, b, 0), color);
            vertices[0] = new ColorVertexData(center + new Vector3(-a, b, 0), color);
            vertices[0] = new ColorVertexData(center + new Vector3(a, -b, 0), color);
            vertices[0] = new ColorVertexData(center + new Vector3(-a, -b, 0), color);
            vertices[0] = new ColorVertexData(center + new Vector3(b, 0, a), color);
            vertices[0] = new ColorVertexData(center + new Vector3(b, 0, -a), color);
            vertices[0] = new ColorVertexData(center + new Vector3(-b, 0, a), color);
            vertices[0] = new ColorVertexData(center + new Vector3(-b, 0, -a), color);
            vertices[0] = new ColorVertexData(center + new Vector3(0, a, b), color);
            vertices[0] = new ColorVertexData(center + new Vector3(0, -a, b), color);
            vertices[0] = new ColorVertexData(center + new Vector3(0, a, -b), color);
            vertices[0] = new ColorVertexData(center + new Vector3(0, -a, -b), color);

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
                11, 7, 5
            };

            copyIndices(localIndices, indices, indexOffset);
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
