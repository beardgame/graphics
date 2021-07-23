using System;
using Bearded.Graphics.MeshBuilders;
using Bearded.Graphics.Vertices;
using OpenTK.Mathematics;

namespace Bearded.Graphics.Shapes
{
    public sealed class ShapeDrawer2<TVertex, TVertexParameters> : IShapeDrawer2<TVertexParameters>
        where TVertex : struct, IVertexData
    {
        public delegate TVertex CreateShapeVertex(Vector3 xyz, TVertexParameters parameters);

        private readonly IIndexedTrianglesMeshBuilder<TVertex, ushort> meshBuilder;
        private readonly CreateShapeVertex createVertex;

        public ShapeDrawer2(
            IIndexedTrianglesMeshBuilder<TVertex, ushort> meshBuilder, CreateShapeVertex createVertex)
        {
            this.meshBuilder = meshBuilder;
            this.createVertex = createVertex;
        }

        public void FillRectangle(float x, float y, float z, float w, float h, TVertexParameters parameters)
        {
            meshBuilder.AddQuad(
                createVertex(new Vector3(x, y, z), parameters),
                createVertex(new Vector3(x + w, y, z), parameters),
                createVertex(new Vector3(x + w, y + h, z), parameters),
                createVertex(new Vector3(x, y + h, z), parameters)
            );
        }

        public void DrawRectangle(
            float x, float y, float z, float w, float h, float lineWidth, TVertexParameters parameters)
        {
            meshBuilder.Add(8, 24, out var vertices, out var indices, out var indexOffset);

            // outer
            vertices[0] = createVertex(new Vector3(x, y, z), parameters);
            vertices[1] = createVertex(new Vector3(x + w, y, z), parameters);
            vertices[2] = createVertex(new Vector3(x + w, y + h, z), parameters);
            vertices[3] = createVertex(new Vector3(x, y + h, z), parameters);

            // inner
            vertices[4] = createVertex(new Vector3(x + lineWidth, y + lineWidth, z), parameters);
            vertices[5] = createVertex(new Vector3(x + w - lineWidth, y + lineWidth, z), parameters);
            vertices[6] = createVertex(new Vector3(x + w - lineWidth, y + h - lineWidth, z), parameters);
            vertices[7] = createVertex(new Vector3(x + lineWidth, y + h - lineWidth, z), parameters);

            var indicesIndex = 0;
            for (var i = 0; i < 4; i++)
            {
                var outer1 = i;
                var outer2 = (i + 1) % 4;
                var inner1 = outer1 + 4;
                var inner2 = outer2 + 4;

                indices[indicesIndex++] = (ushort) (indexOffset + outer1);
                indices[indicesIndex++] = (ushort) (indexOffset + outer2);
                indices[indicesIndex++] = (ushort) (indexOffset + inner2);

                indices[indicesIndex++] = (ushort) (indexOffset + outer1);
                indices[indicesIndex++] = (ushort) (indexOffset + inner2);
                indices[indicesIndex++] = (ushort) (indexOffset + inner1);
            }
        }

        public void FillOval(
            float centerX, float centerY, float centerZ, float radiusX, float radiusY,
            TVertexParameters parameters, int edges)
        {
            if (edges < 3)
                throw new ArgumentOutOfRangeException(nameof(edges), "Must draw at least three edges.");

            meshBuilder.Add(edges, (edges - 2) * 3, out var vertices, out var indices, out var indexOffset);

            var rotation = Matrix2.CreateRotation(MathHelper.TwoPi / edges);

            var xy = new Vector2(0, -1);

            vertices[0] = createVertex(new Vector3(
                centerX + xy.X * radiusX, centerY + xy.Y * radiusY, centerZ), parameters);

            for (var i = 1; i < edges; i++)
            {
                xy = rotation * xy;

                vertices[i] = createVertex(new Vector3(
                    centerX + xy.X * radiusX, centerY + xy.Y * radiusY, centerZ), parameters);
            }

            for (var i = 0; i < edges - 2; i++)
            {
                var o = i * 3;
                indices[o++] = indexOffset;
                indices[o++] = (ushort)(indexOffset + i + 1);
                indices[o] = (ushort)(indexOffset + i + 2);
            }
        }

        public void DrawOval(
            float centerX, float centerY, float centerZ, float radiusX, float radiusY,
            float lineWidth, TVertexParameters parameters, int edges)
        {
            if (edges < 3)
                throw new ArgumentOutOfRangeException(nameof(edges), "Must draw at least three edges.");

            meshBuilder.Add(edges * 2, edges * 6, out var vertices, out var indices, out var indexOffset);

            var rotation = Matrix2.CreateRotation(MathHelper.TwoPi / edges);

            var xy = new Vector2(0, -1);

            var innerW = radiusX - lineWidth;
            var innerH = radiusY - lineWidth;

            vertices[0] = createVertex(new Vector3(
                centerX + xy.X * radiusX, centerY + xy.Y * radiusY, centerZ), parameters);

            vertices[1] = createVertex(new Vector3(
                centerX + xy.X * innerW, centerY + xy.Y * innerH, centerZ), parameters);

            for (var i = 1; i < edges; i++)
            {
                xy = rotation * xy;

                vertices[2 * i] = createVertex(new Vector3(
                    centerX + xy.X * radiusX, centerY + xy.Y * radiusY, centerZ), parameters);

                vertices[2 * i + 1] = createVertex(new Vector3(
                    centerX + xy.X * innerW, centerY + xy.Y * innerH, centerZ), parameters);
            }

            for (var i = 0; i < edges - 1; i++)
            {
                var j = i * 6;
                var o = i * 2;
                indices[j] = (ushort)(indexOffset + o);
                indices[j + 1] = (ushort)(indexOffset + o + 2);
                indices[j + 2] = (ushort)(indexOffset + o + 1);

                indices[j + 3] = (ushort)(indexOffset + o + 1);
                indices[j + 4] = (ushort)(indexOffset + o + 2);
                indices[j + 5] = (ushort)(indexOffset + o + 3);
            }

            var lastJ = edges * 6 - 6;

            indices[lastJ] = (ushort)(indexOffset + edges * 2 - 2);
            indices[lastJ + 1] = indexOffset;
            indices[lastJ + 2] = (ushort)(indexOffset + edges * 2 - 1);

            indices[lastJ + 3] = (ushort)(indexOffset + edges * 2 - 1);
            indices[lastJ + 4] = indexOffset;
            indices[lastJ + 5] = (ushort)(indexOffset + 1);
        }

        public void DrawLine(
            float x1, float y1, float z1, float x2, float y2, float z2, float lineWidth, TVertexParameters parameters)
        {
            var vx = x2 - x1;
            var vy = y1 - y2; // switch order for correct normal direction
            var ilxy = 0.5f * lineWidth / (float)Math.Sqrt(vx * vx + vy * vy);
            var nx = vy * ilxy;
            var ny = vx * ilxy;
            meshBuilder.AddQuad(
                createVertex(new Vector3(x1 + nx, y1 + ny, z1), parameters),
                createVertex(new Vector3(x1 - nx, y1 - ny, z1), parameters),
                createVertex(new Vector3(x2 - nx, y2 - ny, z2), parameters),
                createVertex(new Vector3(x2 + nx, y2 + ny, z2), parameters)
                );
        }
    }
}
