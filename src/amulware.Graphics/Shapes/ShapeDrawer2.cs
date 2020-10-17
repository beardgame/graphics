using System;
using amulware.Graphics.MeshBuilders;
using amulware.Graphics.Vertices;
using OpenToolkit.Mathematics;

namespace amulware.Graphics.Shapes
{
    public sealed class ShapeDrawer2<TVertex, TVertexParameters>
        where TVertex : struct, IVertexData
    {
        public delegate TVertex CreateShapeVertex(Vector3 xyz, TVertexParameters parameters);

        private readonly IIndexedTrianglesMeshBuilder<TVertex, ushort> meshBuilder;
        private readonly CreateShapeVertex createShapeVertex;

        public ShapeDrawer2(
            IIndexedTrianglesMeshBuilder<TVertex, ushort> meshBuilder, CreateShapeVertex createShapeVertex)
        {
            this.meshBuilder = meshBuilder;
            this.createShapeVertex = createShapeVertex;
        }

        public void FillRectangle(Vector2 xy, Vector2 wh, TVertexParameters parameters)
        {
            FillRectangle(xy.X, xy.Y, 0, wh.X, wh.Y, parameters);
        }

        public void FillRectangle(Vector3 xyz, Vector2 wh, TVertexParameters parameters)
        {
            FillRectangle(xyz.X, xyz.Y, xyz.Z, wh.X, wh.Y, parameters);
        }

        public void FillRectangle(float x, float y, float w, float h, TVertexParameters parameters)
        {
            FillRectangle(x, y, 0, w, h, parameters);
        }

        public void FillRectangle(float x, float y, float z, float w, float h, TVertexParameters parameters)
        {
            meshBuilder.AddQuad(
                createShapeVertex(new Vector3(x, y, z), parameters),
                createShapeVertex(new Vector3(x + w, y, z), parameters),
                createShapeVertex(new Vector3(x + w, y + h, z), parameters),
                createShapeVertex(new Vector3(x, y + h, z), parameters)
            );
        }

        public void DrawRectangle(Vector2 xy, Vector2 wh, float lineWidth, TVertexParameters parameters)
        {
            DrawRectangle(xy.X, xy.Y, 0, wh.X, wh.Y, lineWidth, parameters);
        }
        public void DrawRectangle(Vector3 xyz, Vector2 wh, float lineWidth, TVertexParameters parameters)
        {
            DrawRectangle(xyz.X, xyz.Y, xyz.Z, wh.X, wh.Y, lineWidth, parameters);
        }
        public void DrawRectangle(float x, float y, float w, float h, float lineWidth, TVertexParameters parameters)
        {
            DrawRectangle(x, y, 0, w, h, lineWidth, parameters);
        }

        public void DrawRectangle(float x, float y, float z, float w, float h, float lineWidth, TVertexParameters parameters)
        {
            meshBuilder.Add(8, 24, out var vertices, out var indices, out var indexOffset);

            // outer
            vertices[0] = createShapeVertex(new Vector3(x, y, z), parameters);
            vertices[1] = createShapeVertex(new Vector3(x + w, y, z), parameters);
            vertices[2] = createShapeVertex(new Vector3(x + w, y + h, z), parameters);
            vertices[3] = createShapeVertex(new Vector3(x, y + h, z), parameters);

            // inner
            vertices[4] = createShapeVertex(new Vector3(x + lineWidth, y + lineWidth, z), parameters);
            vertices[5] = createShapeVertex(new Vector3(x + w - lineWidth, y + lineWidth, z), parameters);
            vertices[6] = createShapeVertex(new Vector3(x + w - lineWidth, y + h - lineWidth, z), parameters);
            vertices[7] = createShapeVertex(new Vector3(x + lineWidth, y + h - lineWidth, z), parameters);

            var indicesIndex = 0;
            for (var i = 0; i < 4; i++)
            {
                indices[indicesIndex++] = (ushort) (indexOffset + i);
                indices[indicesIndex++] = (ushort) (indexOffset + (i + 1) % 4);
                indices[indicesIndex++] = (ushort) (indexOffset + (i + 1) % 4 + 4);

                indices[indicesIndex++] = (ushort) (indexOffset + i);
                indices[indicesIndex++] = (ushort) (indexOffset + (i + 3) % 4 + 4);
                indices[indicesIndex++] = (ushort) (indexOffset + (i + 3) % 4);
            }
        }

        public void FillCircle(float x, float y, float r, TVertexParameters parameters, int edges = 32)
        {
            fillOval(x, y, 0, r, r, parameters, edges);
        }

        public void FillCircle(Vector2 xy, float r, TVertexParameters parameters, int edges = 32)
        {
            fillOval(xy.X, xy.Y, 0, r, r, parameters, edges);
        }

        public void FillCircle(float x, float y, float z, float r, TVertexParameters parameters, int edges = 32)
        {
            fillOval(x, y, z, r, r, parameters, edges);
        }

        public void FillCircle(Vector3 xyz, float r, TVertexParameters parameters, int edges = 32)
        {
            fillOval(xyz.X, xyz.Y, xyz.Z, r, r, parameters, edges);
        }

        public void DrawCircle(float x, float y, float r, float lineWidth, TVertexParameters parameters, int edges = 32)
        {
            drawOval(x, y, 0, r, r, lineWidth, parameters, edges);
        }

        public void DrawCircle(Vector2 xy, float r, float lineWidth, TVertexParameters parameters, int edges = 32)
        {
            drawOval(xy.X, xy.Y, 0, r, r, lineWidth, parameters, edges);
        }

        public void DrawCircle(float x, float y, float z, float r, float lineWidth, TVertexParameters parameters, int edges = 32)
        {
            drawOval(x, y, z, r, r, lineWidth, parameters, edges);
        }

        public void DrawCircle(Vector3 xyz, float r, float lineWidth, TVertexParameters parameters, int edges = 32)
        {
            drawOval(xyz.X, xyz.Y, xyz.Z, r, r, lineWidth, parameters, edges);
        }

        public void FillOval(Vector2 xy, Vector2 wh, TVertexParameters parameters, int edges = 32)
        {
            wh *= 0.5f;
            fillOval(xy.X + wh.X, xy.Y + wh.Y, 0, wh.X, wh.Y, parameters, edges);
        }

        public void FillOval(Vector3 xyz, Vector2 wh, TVertexParameters parameters, int edges = 32)
        {
            wh *= 0.5f;
            fillOval(xyz.X + wh.X, xyz.Y + wh.Y, xyz.Z, wh.X, wh.Y, parameters, edges);
        }

        public void FillOval(float x, float y, float w, float h, TVertexParameters parameters, int edges = 32)
        {
            w *= 0.5f;
            h *= 0.5f;
            fillOval(x + w, y + h, 0, w, h, parameters, edges);
        }

        public void FillOval(float x, float y, float z, float w, float h, TVertexParameters parameters, int edges = 32)
        {
            w *= 0.5f;
            h *= 0.5f;
            fillOval(x + w, y + h, z, w, h, parameters, edges);
        }

        public void DrawOval(Vector2 xy, Vector2 wh, float lineWidth, TVertexParameters parameters, int edges = 32)
        {
            wh *= 0.5f;
            drawOval(xy.X + wh.X, xy.Y + wh.Y, 0, wh.X, wh.Y, lineWidth, parameters, edges);
        }

        public void DrawOval(Vector3 xyz, Vector2 wh, float lineWidth, TVertexParameters parameters, int edges = 32)
        {
            wh *= 0.5f;
            drawOval(xyz.X + wh.X, xyz.Y + wh.Y, xyz.Z, wh.X, wh.Y, lineWidth, parameters, edges);
        }

        public void DrawOval(float x, float y, float w, float h, float lineWidth, TVertexParameters parameters, int edges = 32)
        {
            w *= 0.5f;
            h *= 0.5f;
            drawOval(x + w, y + h, 0, w, h, lineWidth, parameters, edges);
        }

        public void DrawOval(float x, float y, float z, float w, float h, float lineWidth, TVertexParameters parameters, int edges = 32)
        {
            w *= 0.5f;
            h *= 0.5f;
            drawOval(x + w, y + h, z, w, h, lineWidth, parameters, edges);
        }

        private void fillOval(
            float centerX, float centerY, float centerZ, float halfWidth, float halfHeight, TVertexParameters parameters, int edges)
        {
            if (edges < 3)
                throw new ArgumentOutOfRangeException(nameof(edges), "Must draw at least three edges.");

            meshBuilder.Add(edges, (edges - 2) * 3, out var vertices, out var indices, out var indexOffset);

            var rotation = Matrix2.CreateRotation(MathHelper.TwoPi / edges);

            var xy = new Vector2(0, -1);

            vertices[0] = createShapeVertex(new Vector3(
                centerX + xy.X * halfWidth, centerY + xy.Y * halfHeight, centerZ), parameters);

            for (var i = 1; i < edges; i++)
            {
                xy = rotation * xy;

                vertices[i] = createShapeVertex(new Vector3(
                    centerX + xy.X * halfWidth, centerY + xy.Y * halfHeight, centerZ), parameters);
            }

            for (var i = 0; i < edges - 2; i++)
            {
                var o = i * 3;
                indices[o++] = indexOffset;
                indices[o++] = (ushort)(indexOffset + i + 1);
                indices[o] = (ushort)(indexOffset + i + 2);
            }
        }

        private void drawOval(
            float centerX, float centerY, float centerZ, float halfWidth, float halfHeight,
            float lineWidth, TVertexParameters parameters, int edges)
        {
            if (edges < 3)
                throw new ArgumentOutOfRangeException(nameof(edges), "Must draw at least three edges.");

            meshBuilder.Add(edges * 2, edges * 6, out var vertices, out var indices, out var indexOffset);

            var rotation = Matrix2.CreateRotation(MathHelper.TwoPi / edges);

            var xy = new Vector2(0, -1);

            var innerW = halfWidth - lineWidth;
            var innerH = halfHeight - lineWidth;

            vertices[0] = createShapeVertex(new Vector3(
                centerX + xy.X * halfWidth, centerY + xy.Y * halfHeight, centerZ), parameters);

            vertices[1] = createShapeVertex(new Vector3(
                centerX + xy.X * innerW, centerY + xy.Y * innerH, centerZ), parameters);

            for (var i = 1; i < edges; i++)
            {
                xy = rotation * xy;

                vertices[2 * i] = createShapeVertex(new Vector3(
                    centerX + xy.X * halfWidth, centerY + xy.Y * halfHeight, centerZ), parameters);

                vertices[2 * i + 1] = createShapeVertex(new Vector3(
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

        public void DrawLine(Vector2 xy1, Vector2 xy2, float lineWidth, TVertexParameters parameters)
        {
            DrawLine(xy1.X, xy1.Y, 0, xy2.X, xy2.Y, 0, lineWidth, parameters);
        }

        public void DrawLine(Vector3 xyz1, Vector3 xyz2, float lineWidth, TVertexParameters parameters)
        {
            DrawLine(xyz1.X, xyz1.Y, xyz1.Z, xyz2.X, xyz2.Y, xyz2.Z, lineWidth, parameters);
        }

        public void DrawLine(float x1, float y1, float x2, float y2, float lineWidth, TVertexParameters parameters)
        {
            DrawLine(x1, y1, 0, x2, y2, 0, lineWidth, parameters);
        }

        public void DrawLine(float x1, float y1, float z1, float x2, float y2, float z2, float lineWidth, TVertexParameters parameters)
        {
            var vx = x2 - x1;
            var vy = y1 - y2; // switch order for correct normal direction
            var ilxy = lineWidth / (float)Math.Sqrt(vx * vx + vy * vy);
            var nx = vy * ilxy;
            var ny = vx * ilxy;
            meshBuilder.AddQuad(
                createShapeVertex(new Vector3(x1 + nx, y1 + ny, z1), parameters),
                createShapeVertex(new Vector3(x1 - nx, y1 - ny, z1), parameters),
                createShapeVertex(new Vector3(x2 - nx, y2 - ny, z2), parameters),
                createShapeVertex(new Vector3(x2 + nx, y2 + ny, z2), parameters)
                );
        }
    }
}
